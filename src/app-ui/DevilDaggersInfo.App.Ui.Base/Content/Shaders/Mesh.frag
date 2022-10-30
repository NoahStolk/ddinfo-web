#version 330 core
in vec2 texCoord;
in vec3 fragPosition;

out vec4 FragColor;

uniform sampler2D textureDiffuse;

void main()
{
	float darkness = clamp(fragPosition.y, -2, -1) + 2;
	FragColor = mix(vec4(0, 0, 0, 1), texture(textureDiffuse, texCoord), darkness);
}
