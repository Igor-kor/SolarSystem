#version 330

uniform samplerBuffer tex; // TBO

in vec2 TexCoord;

out vec4 FragColor;

void main()
{
    int index = int(TexCoord.x * 1024.0); 
    vec4 color = texelFetch(tex, index);
    FragColor = color; 
}
