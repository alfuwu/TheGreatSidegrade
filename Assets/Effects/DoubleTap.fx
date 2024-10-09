sampler uTex : register(s0);
float uProgress : register(c0);
//float4 uShaderSpecificData : register(c1);

float4 BlueShaderFunction(float4 position : SV_POSITION, float2 coords : TEXCOORD0) : COLOR0
{
    float4 col = tex2D(uTex, coords);
    float4 blue = float4(0.7, 0.3, 1.0, 1.0);
    return lerp(blue * col, col, max(0, 1 - (uProgress * 1.141592653589)));
}

float4 RippleShaderFunction(float4 position : SV_POSITION, float2 coords : TEXCOORD0) : COLOR0
{
    float2 cp = -1.0 + 2.0 * coords;
    float cl = length(cp);
    float2 uv = coords + (cp / cl) * cos(cl * 12.0 - uProgress * 4.0) * 0.02;
    float3 col = tex2D(uTex, uv).rgb;
    return float4(col, 1.0);
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
