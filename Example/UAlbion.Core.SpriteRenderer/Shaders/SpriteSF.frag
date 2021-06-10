//!#version 450 // Comments with //! are for tricking the Visual Studio GLSL plugin into doing the right thing
//!#extension GL_KHR_vulkan_glsl: enable

// Resource Sets
layout(binding = 0) uniform texture2DArray uSprite;
layout(binding = 1) uniform sampler uSpriteSampler;

layout(binding = 2) uniform _Uniform {
	uint uFlags;
	float uTexSizeW;
	float uTexSizeH;
	uint _u_padding_3;
};

// Shared set
#include "CommonResources.glsl"

// Inputs from vertex shader
layout(location = 0) in vec2 iTexPosition;   // Texture Coordinates
layout(location = 1) in flat float iLayer;   // Texture Layer
layout(location = 2) in flat uint  iFlags;   // Flags
layout(location = 3) in vec2 iNormCoords;    // Normalised sprite coordinates
layout(location = 4) in vec3 iWorldPosition; // World-space position

// Fragment shader outputs
layout(location = 0) out vec4 OutputColor;

void main()
{
	vec2 screenCoords = gl_FragCoord.xy / uResolution;
	vec2 uv = ((iFlags & SF_FLIP_VERTICAL) != 0) 
		? vec2(iTexPosition.x, 1 - iTexPosition.y) 
		: iTexPosition;

	vec4 color = texture(sampler2DArray(uSprite, uSpriteSampler), vec3(uv, iLayer)); //! vec4 color;

//#ifdef USE_PALETTE
	color = color[0] == 0 
		? vec4(0)
		: texture(sampler2D(uPalette, uSpriteSampler),
			vec2((color[0] * 255.0f/256.f) + (0.5f/256.0f), 0));
//#endif
/*
	if ((iFlags & SF_GRADIENT_PIXELS) != 0)
	{
		vec2 subPixelPos = smoothstep(
			0,
			1,
			1 - fract(uv * vec2(uTexSizeW, uTexSizeH)));
		color = color * vec4(
			vec3(subPixelPos.x*subPixelPos.y + 0.4),
			1.0);
	}

#ifdef DEBUG
	// Outline
	if ((uEngineFlags & EF_SHOW_BOUNDING_BOXES) != 0 && (iFlags & SF_NO_BOUNDING_BOX) == 0)
	{
		vec2 factor = step(vec2(0.02), min(iNormCoords, 1.0f - iNormCoords));
		color = mix(color, vec4(1.0f), vec4(1.0f - min(factor.x, factor.y)));
	}

	if ((uEngineFlags & EF_SHOW_CENTRE) != 0)
	{
		float dist = length(vec3(screenCoords, 0) - vec3(0.5, 0.5, 0));
		if (dist < 0.01)
			color = mix(color, vec4(1.0f, 0.0f, 0.0f, 1.0f), vec4(0.4));
	}

	if ((iFlags & SF_HIGHLIGHT)  != 0) color = color * 1.2;
	if ((iFlags & SF_RED_TINT)   != 0) color = vec4(color.x * 1.5f + 0.3f, color.yz * 0.7f,                       color.w);
	if ((iFlags & SF_GREEN_TINT) != 0) color = vec4(color.x * 0.7f,        color.y * 1.5f + 0.3f, color.z * 0.7f, color.w);
	if ((iFlags & SF_BLUE_TINT)  != 0) color = vec4(color.xy * 0.7f,       color.z * 1.5f + 0.3f,                 color.w);
#endif
*/
	float depth = (color.w == 0.0f) ? 1.0f : gl_FragCoord.z;
/*
	if ((iFlags & SF_DROP_SHADOW) != 0)
		color = vec4(0.0f, 0.0f, 0.0f, 1.0f);

	if ((iFlags & 0xff000000) != 0) // High order byte = opacity
	{
		float opacity = (((iFlags & 0xff000000) >> 24) / 255.0f);
		color = vec4(color.xyz, color.w * opacity);
	}


#ifdef DEBUG
	if ((uEngineFlags & EF_RENDER_DEPTH) != 0)
		color = DEPTH_COLOR(depth);
#endif
*/	
	OutputColor = color;
	gl_FragDepth = ((uEngineFlags & EF_FLIP_DEPTH_RANGE) != 0) ? 1.0f - depth : depth;
}
