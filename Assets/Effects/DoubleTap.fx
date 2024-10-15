sampler uTex : register(s0);
float uProgress : register(c0);
float2 uScreenResolution : register(c1);

float4 BlueShaderFunction(float4 position : SV_POSITION, float2 coords : TEXCOORD0) : COLOR0
{
    float p = uProgress > 0.9 ? 1 - (uProgress - 0.9) * 10 : uProgress;
    float4 col = tex2D(uTex, coords);
    float4 blue = float4(0.7, 0.3, 1.0, 1.0);
    return lerp(blue * col, col, max(0, 1 - (p * 1.141592653589)));
}

float4 OldRippleShaderFunction(float4 position : SV_POSITION, float2 coords : TEXCOORD0) : COLOR0
{
    float p = uProgress > 0.9 ? 1 - (uProgress - 0.9) * 10 : uProgress;
    float2 cp = -1.0 + 2.0 * coords;
    float cl = length(cp);
    float2 uv = coords + (cp / cl) * cos(cl * 12.0 - p * 4.0) * 0.02;
    float3 col = tex2D(uTex, uv).rgb;
    return float4(col, 1.0);
}

float4 RippleShaderFunction(float4 position : SV_POSITION, float2 coords : TEXCOORD0) : COLOR0
{
    float p = uProgress > 0.9 ? 1 - (uProgress - 0.9) * 10 : uProgress;
    float2 center = float2(0.5, 0.5);
    float spread = p;
    float amount = 0.1;
    float width = 0.1;
    float outerMap = 1.0 - smoothstep(spread - width, spread, length(coords - center));
    float innerMap = smoothstep(spread - width * 2, spread - width, length(coords - center));
    float map = outerMap * innerMap;
    float2 displacement = normalize((coords - center) * float2(uScreenResolution.y / uScreenResolution.x, uScreenResolution.x / uScreenResolution.y)) * amount * map;
    float4 color = tex2D(uTex, coords - displacement);
    return color;
}

technique Technique1
{
    pass BluePass
    {
        AlphaBlendEnable = TRUE;
        BlendOp = ADD;
        SrcBlend = SRCALPHA;
        DestBlend = INVSRCALPHA;
        PixelShader = compile ps_2_0 BlueShaderFunction();
    }

    pass ScreenPass
    {
        AlphaBlendEnable = TRUE;
        BlendOp = ADD;
        SrcBlend = SRCALPHA;
        DestBlend = INVSRCALPHA;
        PixelShader = compile ps_2_0 RippleShaderFunction();
    }
}
