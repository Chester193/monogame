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
Texture2D Heffect_1, Heffect_2, Heffect_3;
float Timer;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

sampler2D Meffect1Tsampl = sampler_state
{
    Texture = <Meffect_1>;
};
sampler2D MeffectTsampl2 = sampler_state
{
    Texture = <Meffect_2>;
};
sampler2D MeffectTsampl3 = sampler_state
{
    Texture = <Meffect_3>;
};

sampler2D Heffect1Tsampl = sampler_state
{
    Texture = <Heffect_1>;
};
sampler2D HeffectTsampl2 = sampler_state
{
    Texture = <Heffect_2>;
};
sampler2D HeffectTsampl3 = sampler_state
{
    Texture = <Heffect_3>;
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
	
    float4 efect1[6];
    efect1[0] = tex2D(Meffect1Tsampl, input.TextureCoordinates);
    efect1[1] = tex2D(MeffectTsampl2, input.TextureCoordinates);
    efect1[2] = tex2D(MeffectTsampl3, input.TextureCoordinates);
	efect1[3] = tex2D(Heffect1Tsampl, input.TextureCoordinates);
    efect1[4] = tex2D(HeffectTsampl2, input.TextureCoordinates);
    efect1[5] = tex2D(HeffectTsampl3, input.TextureCoordinates);
	
    float4 efect2[6];
    efect2[0] = tex2D(Meffect1Tsampl, input.TextureCoordinates);
    efect2[1] = tex2D(MeffectTsampl2, input.TextureCoordinates);
    efect2[2] = tex2D(MeffectTsampl3, input.TextureCoordinates);
    efect2[3] = tex2D(Heffect1Tsampl, input.TextureCoordinates);
    efect2[4] = tex2D(HeffectTsampl2, input.TextureCoordinates);
    efect2[5] = tex2D(HeffectTsampl3, input.TextureCoordinates);
	
    efect2[0].gb = 0;
    efect2[1].gb = 0;
    efect2[2].gb = 0;
    efect2[3].gb = 0;
    efect2[4].gb = 0;
    efect2[5].gb = 0;
	
    efect2[0].r = efect1[0].b;
    efect2[1].r = efect1[1].b;
    efect2[2].r = efect1[2].b;
    efect2[3].r = efect1[3].b;
    efect2[4].r = efect1[4].b;
    efect2[5].r = efect1[5].b;
	
	
	//float value = color.r * 0.299 + color.g * 0.587 + color.b * 0.114;
	//color.rgb = value;
	if (on)
	{
        int var = (int) (Timer % 6);
		if (mPotion)
		{
            color.rgba = efect1[var].rgba;

		}
		else
		{
			//efekt dla mikstury życia
			
            color.rgba = efect2[var].rgba;
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