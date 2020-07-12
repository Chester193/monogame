#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
float playerHealth;

sampler2D InputTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(InputTextureSampler, input.TextureCoordinates) * input.Color;
    color.gb = 0;
    color.r *= playerHealth < 20 ? (20 - playerHealth) / 20 : 0;
    color.a = 0.1;
    return color;
}

technique Techninque1
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};