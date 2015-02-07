Shader "Custom/billBOARD" {
Properties {
      _MainTex ("Texture Image", 2D) = "white" {}
   }
   SubShader {
   Tags {"LightMode" = "ForwardBase"}
      Pass {   
         ZWrite on 	  // don't write to depth buffer -> Whn off my planes are behind.
            // in order not to occlude other objects
         Blend SrcAlpha OneMinusSrcAlpha // alpha blending
           CGPROGRAM     
         #pragma vertex vert  
         #pragma fragment frag 
         
        // float4 viewPosition = float4(_WorldSpaceCameraPos,1);
         // User-specified uniforms            
         uniform sampler2D _MainTex; 
    //     string zonoff = off;
                //  float4 viewPosition = float4(_WorldSpaceCameraPos,1);
  
            struct vertexInput {
            float4 vertex : POSITION;
            float4 tex : TEXCOORD0;   
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 tex : TEXCOORD0; 
         };
         vertexOutput vert(vertexInput input) 
         { 
            vertexOutput output;
           output.pos = mul(UNITY_MATRIX_MVP, input.vertex); // "normal" firkant/cube          
            output.tex = input.tex;

            //Distance Imellem -PLayer og test -> Cube 
      //	float3 afstandvector;
      	//	afstandvector = _WorldSpaceCameraPos.xyz - output.pos;
		 float afstand = length(_WorldSpaceCameraPos.xyz - output.pos.xyz);
		// Debug.Log("FMK");
		 if(9.0f<afstand){
		       output.pos = mul(UNITY_MATRIX_P, 
              mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0)) //det her er fladen.
              - float4(input.vertex.x, input.vertex.z, 0.0, 0.0));                
            }       
              return output;       
         }
         float4 frag(vertexOutput input) : COLOR
         {
            return tex2D(_MainTex, float2(input.tex.xy));        		
		 }
         ENDCG
      }
   }
}
