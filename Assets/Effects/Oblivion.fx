sampler uNoiseTex : register(s0);
sampler uVignette : register(s1);
float4 uShaderSpecificData : register(c0);
float uTime : register(c1);
float3 uBrighterCol = float3(1.0, 1.0, 1.0); // white
float3 uMiddleCol = float3(0.7, 0.7, 0.7); // gray
float3 uDarkerCol = float3(0.1, 0.1, 0.1); // black

float4 PixelShaderFunction(float4 position : SV_POSITION, float2 coords : TEXCOORD0) : COLOR0
{
    // pixelization
    coords = round(float2(coords.x, coords.y) * 150) / 150;
    
    // get center pos
    float2 centeredCoords = coords - float2(0.5, 0.5);

    // calc distance to center, use pow to favor center positions over distant positions
    float distanceFromCenter = pow(length(centeredCoords), 0.8);

    float2 acoords = float2(abs((coords.x + sin(uTime) * 0.1) % 1.0), (coords.y + uTime) % 1.0);
    // sample noise texture
    float noiseValue = tex2D(uNoiseTex, acoords).x * pow(max(0.8, tex2D(uVignette, acoords).x), 2); // woosh
    // oops accidentally used acoords for vignette instead of coords, but i kinda like the pulsing effect so it stays

    // use the radial distance as the gradient effect
    float gradientValue = 1.0 - distanceFromCenter * 1.6;

    // step calcs
    float step1 = step(noiseValue, gradientValue);
    float step2 = step(noiseValue, gradientValue - 0.2);
    float step3 = step(noiseValue, gradientValue - 0.4);

    // base color
    float3 color = lerp(uBrighterCol, uDarkerCol, step1 - step2);
    
    // middle color blending
    color = lerp(color, uMiddleCol, step2 - step3);

    // add alpha
    return float4(color, step1);
}

technique Technique1
{
    pass ScreenPass
    {
        AlphaBlendEnable = TRUE;
        BlendOp = ADD;
        SrcBlend = SRCALPHA;
        DestBlend = INVSRCALPHA;
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
