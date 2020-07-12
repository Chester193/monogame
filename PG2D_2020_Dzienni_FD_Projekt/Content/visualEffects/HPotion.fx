#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

bool on;
bool mPotion;

Texture2D SpriteTexture;

Texture2D Meffect_1, Meffect_2, Meffect_3;
float Timer;

sampler2D SpriteTextureSampler = sampler_state
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
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
	
	//float value = color.r * 0.299 + color.g * 0.587 + color.b * 0.114;
	//color.rgb = value;
	if (on)
	{
		int var = (int)Timer / 30;
		if (mPotion)
		{
			switch (var)
			{
				
				
			}

		}
		else
		{
			//efekt dla mikstury życia
			
			color.r = color.b;
			color.b = 0;
			//color.a = 0.1;
		}
			
	}
	else
	{
		color.rgba = 0;
	}
		
	
		return color;
	}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};