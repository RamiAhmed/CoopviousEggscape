Shader "Custom/tryingopacity" {   
        Properties
        {
            _MainTex ("Default Texture", 2D) = "White" {}
            _Scale("Scale", float) = 1.0
            _Transparency("Transparency", Range(0.0,1.0)) = 1.0
            _Tint("Tint Color", Color) = (1.0,1.0,1.0,1.0) 
        }
        
            SubShader
            {
            Tags { "Queue"="Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha
            AlphaTest Greater .01 //cuts off low-alpha pixels
            Cull Back

                Pass
                {
                    CGPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag
                   
                   	//User defined variables
                    sampler2D _MainTex;
                    float _Scale;
                    float _Transparency;
                    float4 _Tint;
                    
                    //Structs
                    struct vertexInput
                    {
                        float4 vertex : POSITION;
                        float4 color : COLOR;
                        float2 texcoord : TEXCOORD0;
                    };
       
                    struct vertexOutput
                    {
                        float4 vertex : POSITION;
                        fixed4 color : COLOR;
                        float2 texcoord : TEXCOORD0;
                    };
       
                    vertexOutput vert (vertexInput v)
                    {
                        vertexOutput o;
						o.color = v.color;
                        o.vertex = mul(UNITY_MATRIX_P, 		mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0)) + float4(v.vertex.x, v.vertex.y, 0.0, 1.0)*_Scale		);
                        o.texcoord = float2(v.vertex.x  + 0.5, v.vertex.y + 0.5);                                           
                        return o;
                    }
                    
       				
                    float4 frag (vertexOutput i) : COLOR
                    {                       
                        return i.color * tex2D(_MainTex, i.texcoord) * _Transparency * _Tint;

                    }
                    
                    ENDCG
                }
              
        }
    }