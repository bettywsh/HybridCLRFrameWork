// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.05
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.05;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:2,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:False,dith:0,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:3256,x:32842,y:32677,varname:node_3256,prsc:2|emission-3344-OUT;n:type:ShaderForge.SFN_Tex2d,id:8626,x:31950,y:32798,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_8626,prsc:2,ntxv:0,isnm:False|UVIN-2101-UVOUT;n:type:ShaderForge.SFN_Panner,id:2101,x:31758,y:32776,varname:node_2101,prsc:2,spu:0,spv:0|UVIN-5741-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:5741,x:31474,y:32763,varname:node_5741,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2d,id:8037,x:31950,y:33020,ptovrint:False,ptlb:Apla,ptin:_Apla,varname:node_8037,prsc:2,ntxv:0,isnm:False|UVIN-6992-UVOUT;n:type:ShaderForge.SFN_Panner,id:6992,x:31715,y:33020,varname:node_6992,prsc:2,spu:0,spv:0|UVIN-9935-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:9935,x:31538,y:32989,varname:node_9935,prsc:2,uv:0;n:type:ShaderForge.SFN_Color,id:6363,x:31934,y:32607,ptovrint:False,ptlb:color,ptin:_color,varname:node_6363,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:7609,x:32267,y:32775,varname:node_7609,prsc:2|A-6363-RGB,B-8626-RGB,C-8037-RGB,D-1713-RGB;n:type:ShaderForge.SFN_Multiply,id:7973,x:32267,y:33027,varname:node_7973,prsc:2|A-6363-A,B-8626-A,C-8037-A,D-1713-A;n:type:ShaderForge.SFN_ValueProperty,id:7258,x:32519,y:33027,ptovrint:False,ptlb:power,ptin:_power,varname:node_7258,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:3344,x:32624,y:32697,varname:node_3344,prsc:2|A-7609-OUT,B-7973-OUT,C-7258-OUT;n:type:ShaderForge.SFN_VertexColor,id:1713,x:31966,y:33245,varname:node_1713,prsc:2;proporder:6363-7258-8626-8037;pass:END;sub:END;*/

Shader "effect/2layerUV_add" {
    Properties {
        _color ("color", Color) = (0.5,0.5,0.5,1)
        _power ("power", Float ) = 1
        _Texture ("Texture", 2D) = "white" {}
        _Apla ("Apla", 2D) = "white" {}
        texA_x("tex_x",float) = 0
        texA_y("tex_y",float) = 0
        texB_x("Apla_x",float) = 0
        texB_y("Apla_y",float) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Blend One One
            Cull Off
            ZWrite Off

            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            //#pragma exclude_renderers xbox360 ps3 flash d3d11_9x
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform sampler2D _Apla; uniform float4 _Apla_ST;
            uniform float4 _color;
            uniform float _power;
            uniform float texA_x;
            uniform float texA_y;
            uniform float texB_x;
            uniform float texB_y;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float4 node_3584 = _Time + _TimeEditor;
                float2 node_2101 = (i.uv0+node_3584.g*float2(texA_x,texA_y));
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(node_2101, _Texture));
                float2 node_6992 = (i.uv0+node_3584.g*float2(texB_x,texB_y));
                float4 _Apla_var = tex2D(_Apla,TRANSFORM_TEX(node_6992, _Apla));
                float3 node_7609 = (_color.rgb*_Texture_var.rgb*_Apla_var.rgb*i.vertexColor.rgb);
                float node_7973 = (_color.a*_Texture_var.a*_Apla_var.a*i.vertexColor.a);
                float3 emissive = (node_7609*node_7973*_power);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
