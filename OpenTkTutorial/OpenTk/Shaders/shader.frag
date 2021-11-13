#version 330

in vec4 FragInColor;
in vec2 FragInTextureCoordinates;

out vec4 FragOutColor;

uniform float hasTexture;
uniform sampler2D textureUnit0;

void main()
{
    if(hasTexture > 0.5f)
    {
        FragOutColor = texture(textureUnit0, FragInTextureCoordinates) * FragInColor;
    }
    else
    {
        FragOutColor = FragInColor;
    };
}