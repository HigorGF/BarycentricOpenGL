uniform sampler2D iChannel0;
uniform sampler2D iChannel1;
uniform sampler2D iChannel2;
uniform float iTime;
uniform vec2 iResolution;
uniform vec4 iMouse;
#define EPS  .01
#define COL0 vec3(.2, .35, .55)
#define COL1 vec3(.9, .43, .34)
#define COL2 vec3(.96, .66, .13)
#define COL3 vec3(0.0)
#define COL4 vec3(0.99,0.1,0.09)




/*  Install  Istructions

sudo apt-get install g++ cmake git
 sudo apt-get install libsoil-dev libglm-dev libassimp-dev libglew-dev libglfw3-dev libxinerama-dev libxcursor-dev
libxi-dev libfreetype-dev libgl1-mesa-dev xorg-dev

git clone https://github.com/JoeyDeVries/LearnOpenGL.git*/

float df_circ(in vec2 p, in vec2 c, in float r)
{
    return abs(r - length(p - c));
}

// Find the intersection of "p" onto "ab".
vec2 intersect (vec2 p, vec2 a, vec2 b)
{
    // Calculate the unit vector from "a" to "b".
    vec2 ba = normalize(b - a);

    // Calculate the intersection of p onto "ab" by
    // calculating the dot product between the unit vector
    // "ba" and the direction vector from "a" to "p", then
    // this value is multiplied by the unit vector "ab"
    // fired from the point "a".
    return a + ba * dot(ba, p - a);
}


// Visual line for debugging purposes.
bool line (vec2 p, vec2 a, vec2 b)
{
    // Direction from a to b.
    vec2 ab = normalize(b - a);

    // Direction from a to the pixel.
    vec2 ap = p - a;

    // Find the intersection of the pixel on to vector
    // from a to b, calculate the distance between the
    // pixel and the intersection point, then compare
    // that distance to the line width.
    return length((a + ab * dot(ab, ap)) - p) < 0.0025;
}

float df_line(in vec2 p, in vec2 a, in vec2 b)
{
    vec2 pa = p - a, ba = b - a;
        float h = clamp(dot(pa,ba) / dot(ba,ba), 0., 1.);
        return length(pa - ba * h);
}

float sharpen(in float d, in float w)
{
    float e = 1. / min(iResolution.y , iResolution.x);
    return 1. - smoothstep(-e, e, d - w);
}


//Editei essa função
vec3 bary(in vec3 a, in vec3 b, in vec3 c, in vec3 p)
{
    float atotal = (b.z-c.z)*(a.x-c.x) + (c.x-b.x)*(a.z-c.z);
    float lamb1 = ((b.z-c.z)*(p.x-c.x) + (c.x-b.x)*(p.z-c.z))/atotal;
    float lamb2 = ((c.z-a.z)*(p.x-c.x) + (a.x-c.x)*(p.z-c.z))/atotal;
    float lamb3 = 1.0 - (lamb1 + lamb2);//Porque a soma tem que dar 1

    return vec3(lamb1,lamb2,lamb3);
}

bool test(in vec2 a, in vec2 b, in vec2 c, in vec2 p, inout vec3 barycoords)
{
    barycoords = bary(vec3(a.x, 0., a.y),
                  vec3(b.x, 0., b.y),
                  vec3(c.x, 0., c.y),
                  vec3(p.x, 0., p.y));

    return barycoords.x > 0. && barycoords.y > 0. && barycoords.z > 0.;
}


float df_bounds(in vec2 uv, in vec2 p, in vec2 a, in vec2 b, in vec2 c, in vec3 barycoords)
{
    float cp = 0.;


    return cp;
}

/*vec3 scene(in vec2 uv, in vec2 a, in vec2 b, in vec2 c, in vec2 p)
{
    float d = df_bounds(uv, p, a, b, c);
    return d > 0. ? COL3 : COL1;
}*/

vec3 globalColor (in vec2 uv, in vec2 a, in vec2 b, in vec2 c)
{
    vec3 r=vec3(0.0);

    return r;
}

void main()
{

    //df_bounds(uv, mc, a, b, c,barycoords);

    float ar = iResolution.x / iResolution.y;
        vec2 mc=vec2(0.0);
        vec2 uv = (gl_FragCoord.xy / iResolution.xy * 2. - 1.) * vec2(ar, 1.);
            if(iMouse.z==1.0)
             mc = (iMouse.xy    / iResolution.xy * 2. - 1.) * vec2(ar, 1.);


    vec2 a = vec2( .73,  .75);
    vec2 b = vec2(-.85,  .15);
    vec2 c = vec2( .25, -.75);
    

    
    vec3 barycoords;
    bool t0 = test(a, b, c, mc,barycoords);
    
    vec3 barycoords2;
    bool t1 = test(a, b, c, uv,barycoords2);



    float g = 0.0;


    vec3 r = globalColor(uv,a,b,c);
    bool testcc = t1;
    
    vec3 color = vec3(0.0); //Cor padrão é preta

    //Variavel para facilitar
    vec3 Vermei = vec3(1.0, 0.4, 0.0); 
    vec3 Azu = vec3(0.2, 0.2, 0.75); 
    vec3 Preto = vec3(0.0, 0.0, 0.0);
    vec3 Branco = vec3(1.0, 1.0, 1.0);

    if(t1 && t0) //Cor do triangulo natural
        color = mix(Preto, Branco, gl_FragCoord.x/800);
        

    else if (t1) //Cor do triangulo se eu clicar fora
        color = mix(Vermei, Azu,  1- gl_FragCoord.x/800); //eu coloquei 1- só para inverter e destacar o triangulo
        

    else if (t0) //Cor de todo resto natural 
        color = mix(Vermei, Azu, gl_FragCoord.x/800);

    //Se eu clicar a parte de fora volta pra cor padrão e vai ficar preto


    // Visual debug lines and points.
        if (line(uv, a, b))
           color = vec3(1.0, 1.0, 1.0);

        if (line(uv, b, c))
           color = vec3(1.0, 1.0, 1.0);

        if (line(uv, c, a))
           color = vec3(1.0, 1.0, 1.0);
    

  

       if (barycoords.z < 0 && df_line(uv, a, b) < 0.06*(barycoords.x + barycoords.y))
           color = vec3(1.0, 1.0, 1.0);

       if (barycoords.x < 0 && df_line(uv, b, c) < 0.06*(barycoords.z + barycoords.y))
           color = vec3(1.0, 1.0, 1.0);

       if (barycoords.y < 0 &&  df_line(uv, c, a) < 0.06*(barycoords.x + barycoords.z))
           color = vec3(1.0, 1.0, 1.0);
    




      if (df_circ(uv, a,EPS)<0.5*EPS)
          color = vec3(0.0, 1.0, 0.0);
      if (df_circ(uv, b,EPS)<0.5*EPS)
          color = vec3(1.0, 0.0, 0.0);
      if (df_circ(uv, c,EPS)<0.5*EPS)
          color = vec3(0.0, 0.0, 1.0);

    //vec3 col = l > 0. ? (testcc ? color : vec3(1)-color) : (t1 ? r : (t0 ? COL3+color : COL2-color));

    //gl_FragColor = vec4(1.0-col, 1);

    //Comentei essa parte de cima pq n tinha a menor ideia do que fazia e tava me confundindo

    gl_FragColor = vec4(color,1);
}
