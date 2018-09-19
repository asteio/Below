//All code and graphical content is 100% originally created by the Shark Boys group
//aside from the methods and classes used directly from the Monogame/XNA Framework

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Below
{
	/// <summary>
	/// 2d deferred lighting
	/// </summary>
	public class LightingEngine
	{
		private GraphicsDevice graphicsDevice;
		private RenderTarget2D normalMapRenderTarget;
		private RenderTarget2D colorMapRenderTarget;
		private RenderTarget2D lightMapRenderTarget;

		private VertexBuffer screenQuad;
		private List<Light> lights = new List<Light>();

		private Effect lightEffect;
		private Effect deferredLightEffect;

		private EffectTechnique lightEffectTechniquePointLight;
		private EffectTechnique lightEffectTechniqueSpotLight;
		private EffectParameter lightEffectParameterStrength;
		private EffectParameter lightEffectParameterPosition;
		private EffectParameter lightEffectParameterLightColor;
		private EffectParameter lightEffectParameterLightDecay;
		private EffectParameter lightEffectParameterScreenWidth;
		private EffectParameter lightEffectParameterScreenHeight;
		private EffectParameter lightEffectParameterNormapMap;
		private EffectParameter lightEffectParameterInvertY;
		private EffectParameter lightEffectParameterAmbientColor;
		private EffectParameter lightEffectParameterColormap;
		private EffectParameter lightEffectParameterSpecularStrength;

		private EffectParameter lightEffectParameterConeAngle;
		private EffectParameter lightEffectParameterConeDecay;
		private EffectParameter lightEffectParameterConeDirection;

		private EffectTechnique deferredLightEffectTechnique;
		private EffectParameter deferredLightEffectParamAmbient;
		private EffectParameter deferredLightEffectParamLightAmbient;
		private EffectParameter deferredLightEffectParamAmbientColor;
		private EffectParameter deferredLightEffectParamColorMap;
		private EffectParameter deferredLightEffectParamShadowMap;
		private EffectParameter deferredLightEffectParamNormalMap;

		/// <summary>
		/// The color of the ambient light that affects the scene.
		/// </summary>
		public Color AmbientLight = new Color(1f, 1f, 1f, 1f);

		/// <summary>
		/// How much the AmbientLight affects the scene.
		/// </summary>
		public float AmbientLightPower = 0.3f;
		/// <summary>
		/// most commonly used light levels
		/// </summary>
		/// <param name="level"></param>
		public void SetLightPower(int level)
		{
			switch (level)
			{
				case 0:
					AmbientLightPower = 0f;
					break;
				case 1:
					AmbientLightPower = 0.03f;
					break;
				case 2:
					AmbientLightPower = 0.1f;
					break;
				case 3:
					AmbientLightPower = 0.3f;
					break;
				case 4:
					AmbientLightPower = 0.5f;
					break;
				case 5:
					AmbientLightPower = 0.7f;
					break;
				case 6:
					AmbientLightPower = 0.9f;
					break;
				default:
					AmbientLightPower = 1f;
					break;
			}
		}
		/// <summary>
		/// set light power to default
		/// </summary>
		public void ResetAmbientLightPower()
		{
			AmbientLightPower = 0.3f;
		}

		/// <summary>
		/// specular strength of lighting
		/// </summary>
		public float SpecularStrength = 1.5f;

		/// <summary>
		/// invert y for normal map
		/// </summary>
		public bool InvertYNormal = true;

		/// <summary>
		/// constructor 
		/// </summary>
		/// <param name="graphicsdevice">create rendertarget and vertexbuffer</param>
		/// <param name="lighteffect">draws lights on the lightmap</param>
		/// <param name="deferredlighteffect">combines the diffuse, normal and lightmap.</param>
		public LightingEngine(GraphicsDevice graphicsdevice, Effect lighteffect, Effect deferredlighteffect)
		{
			graphicsDevice = graphicsdevice;
			lightEffect = lighteffect;
			deferredLightEffect = deferredlighteffect;

			VertexPositionColorTexture[] Vertices = new VertexPositionColorTexture[4];
			Vertices[0] = new VertexPositionColorTexture(new Vector3(-1, 1, 0), Color.White, new Vector2(0, 0));
			Vertices[1] = new VertexPositionColorTexture(new Vector3(1, 1, 0), Color.White, new Vector2(1, 0));
			Vertices[2] = new VertexPositionColorTexture(new Vector3(-1, -1, 0), Color.White, new Vector2(0, 1));
			Vertices[3] = new VertexPositionColorTexture(new Vector3(1, -1, 0), Color.White, new Vector2(1, 1));
			screenQuad = new VertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), Vertices.Length, BufferUsage.None);
			screenQuad.SetData(Vertices);

			PresentationParameters pp = graphicsDevice.PresentationParameters;
			int width = pp.BackBufferWidth;
			int height = pp.BackBufferHeight;
			SurfaceFormat format = pp.BackBufferFormat;

			colorMapRenderTarget = new RenderTarget2D(graphicsDevice, width, height);
			normalMapRenderTarget = new RenderTarget2D(graphicsDevice, width, height);
			lightMapRenderTarget = new RenderTarget2D(graphicsDevice, width, height, false, format, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

			// load point light technique
			lightEffectTechniquePointLight = lightEffect.Techniques["DeferredPointLight"];

			// load spot light technique
			lightEffectTechniqueSpotLight = lightEffect.Techniques["DeferredSpotLight"];

			// load light properties
			lightEffectParameterLightColor = lightEffect.Parameters["lightColor"];
			lightEffectParameterLightDecay = lightEffect.Parameters["lightDecay"];
			lightEffectParameterPosition = lightEffect.Parameters["lightPosition"];
			lightEffectParameterScreenHeight = lightEffect.Parameters["screenHeight"];
			lightEffectParameterScreenWidth = lightEffect.Parameters["screenWidth"];
			lightEffectParameterStrength = lightEffect.Parameters["lightStrength"];
			lightEffectParameterSpecularStrength = lightEffect.Parameters["specularStrength"];
			lightEffectParameterInvertY = lightEffect.Parameters["invertY"];
			lightEffectParameterAmbientColor = lightEffect.Parameters["ambientColor"];
			lightEffectParameterColormap = lightEffect.Parameters["ColorMap"];
			lightEffectParameterNormapMap = lightEffect.Parameters["NormalMap"];

			// load spot light parameters
			lightEffectParameterConeDirection = lightEffect.Parameters["coneDirection"];
			lightEffectParameterConeAngle = lightEffect.Parameters["coneAngle"];
			lightEffectParameterConeDecay = lightEffect.Parameters["coneDecay"];

			// load deferred effect parameters
			deferredLightEffectTechnique = deferredLightEffect.Techniques["DeferredLightEffect"];
			deferredLightEffectParamAmbient = deferredLightEffect.Parameters["ambient"];
			deferredLightEffectParamLightAmbient = deferredLightEffect.Parameters["lightAmbient"];
			deferredLightEffectParamAmbientColor = deferredLightEffect.Parameters["ambientColor"];
			deferredLightEffectParamColorMap = deferredLightEffect.Parameters["ColorMap"];
			deferredLightEffectParamShadowMap = deferredLightEffect.Parameters["ShadingMap"];
			deferredLightEffectParamNormalMap = deferredLightEffect.Parameters["NormalMap"];

		}

		/// <summary>
		/// waht to draw normal data to
		/// </summary>
		public RenderTarget2D Normalmap
		{
			get { return normalMapRenderTarget; }
		}

		/// <summary>
		/// where main art is drawn to 
		/// </summary>
		public RenderTarget2D Colormap
		{
			get { return colorMapRenderTarget; }
		}

		/// <summary>
		/// add a new light
		/// </summary>
		/// <param name="light">The light to be added.</param>
		public void AddLight(Light light)
		{
			lights.Add(light);
		}

		/// <summary>
		/// remove a light
		/// </summary>
		/// <param name="light">Light to remove.</param>
		public void RemoveLight(Light light)
		{
			if (lights.Contains(light))
				lights.Remove(light);
		}

		/// <summary>
		/// Clear all lights
		/// </summary>
		public void ClearLights()
		{
			lights.Clear();
		}

		public Light SceneLight(int index)
		{
			if (index >= 0 && index < lights.Count)
			{
				return lights[index];
			}
			else
			{
				throw new System.Exception("Lightindex out of bounds.");
			}
		}

		/// <summary>
		/// Return the number of lights
		/// </summary>
		public int LightsCount
		{
			get { return lights.Count; }
		}

		/// <summary>
		/// Draw the lighted scene to the rendertarget.
		/// </summary>
		/// <param name="rendertarget">The rendertarget to render the result</param>
		/// <param name="spritebatch">A spritebatch used to render things.</param>
		/// <param name="viewport">optional viewport</param>
		public void Draw(RenderTarget2D rendertarget, SpriteBatch spritebatch, Rectangle? viewport = null)
		{
			//Draw the lights based on the normalmap to the lightmap rendertarget.
			GenerateLightMap(viewport);

			graphicsDevice.SetRenderTarget(rendertarget);
			graphicsDevice.Clear(Color.Black);

			// Draw the combined Maps onto the rendertarget.
			DrawCombinedMaps(spritebatch);
		}

		/// <summary>
		/// combine colormap and normal map to create light map
		/// </summary>
		/// <param name="viewport">optional viewport</param>
		/// <returns></returns>
		private void GenerateLightMap(Rectangle? viewport)
		{
			graphicsDevice.SetRenderTarget(lightMapRenderTarget);
			graphicsDevice.Clear(Color.Transparent);
			graphicsDevice.SetVertexBuffer(screenQuad);

			lightEffectParameterScreenWidth.SetValue((float)graphicsDevice.Viewport.Width);
			lightEffectParameterScreenHeight.SetValue((float)graphicsDevice.Viewport.Height);
			lightEffectParameterAmbientColor.SetValue(AmbientLight.ToVector4());
			lightEffectParameterColormap.SetValue(colorMapRenderTarget);
			lightEffectParameterNormapMap.SetValue(normalMapRenderTarget);
			lightEffectParameterInvertY.SetValue(InvertYNormal);

			Vector3 offset = Vector3.Zero; //offset if the viewport has moved.
			if (viewport.HasValue)
			{
				//Set the offset.
				offset.X = -viewport.Value.X;
				offset.Y = -viewport.Value.Y;
			}


			//Loop through all the lights
			foreach (var light in lights)
			{
				//If a light is not enabled- do not render and continue to the next light.
				if (!light.IsEnabled)
					continue;

				//do culling if the viewport was set.
				if (viewport.HasValue)
				{
					//Culling is now based on a simple boundingbox overlap.

					if (light.LightType == LightType.Point && !new Rectangle((int)(light.Position.X - light.LightDecay), (int)(light.Position.Y - light.LightDecay), (int)(2 * light.LightDecay), (int)(2 * light.LightDecay)).Intersects(viewport.Value))
					{
						//the rectangles do not overlap. Do not draw the light.
						continue;
					}
					if (light.LightType == LightType.Spot)
					{
						//the spotlight is a bit more complicated.
						//for no do a boundingbox check; this should be improved to only hold the actual direction, and width of the beam of the spotlight
						if (!new Rectangle((int)(light.Position.X - (2 * light.LightDecay)), (int)(light.Position.Y - (2 * light.LightDecay)), (int)(4 * light.LightDecay), (int)(4 * light.LightDecay)).Intersects(viewport.Value))
						{
							//the rectangles do not overlap. Do not draw the light.
							continue;
						}

					}

				}

				// Set the values for this lightsource.
				lightEffectParameterStrength.SetValue(light.Power);
				lightEffectParameterPosition.SetValue(light.Position + offset);
				lightEffectParameterLightColor.SetValue(light.Color4);
				lightEffectParameterLightDecay.SetValue(light.LightDecay);
				lightEffectParameterSpecularStrength.SetValue(SpecularStrength);

				if (light.LightType == LightType.Point)
				{
					lightEffect.CurrentTechnique = lightEffectTechniquePointLight;
				}
				else
				{
					lightEffect.CurrentTechnique = lightEffectTechniqueSpotLight;
					lightEffectParameterConeDecay.SetValue(((SpotLight)light).SpotBeamWidthExponent);
					lightEffectParameterConeDirection.SetValue(((SpotLight)light).Direction);
				}


				lightEffect.CurrentTechnique.Passes[0].Apply();

				// Add Black background
				graphicsDevice.BlendState = BlendBlack;

				// Draw the light:
				graphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
			}

			// Complete the drawing
			graphicsDevice.SetRenderTarget(null);

		}

		/// <summary>
		/// draw final combined map
		/// </summary>
		/// <param name="spritebatch">Spritebatch to use in the render process.</param>
		private void DrawCombinedMaps(SpriteBatch spritebatch)
		{
			deferredLightEffect.CurrentTechnique = deferredLightEffectTechnique;
			deferredLightEffectParamAmbient.SetValue(AmbientLightPower);
			deferredLightEffectParamLightAmbient.SetValue(2f);
			deferredLightEffectParamAmbientColor.SetValue(AmbientLight.ToVector4());

			deferredLightEffectParamShadowMap.SetValue(lightMapRenderTarget);
			deferredLightEffectParamNormalMap.SetValue(normalMapRenderTarget);
			deferredLightEffect.CurrentTechnique.Passes[0].Apply();

			spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, deferredLightEffect);
			spritebatch.Draw(colorMapRenderTarget, Vector2.Zero, Color.White);
			spritebatch.End();
		}


		/// <summary>
		/// Blendstate used to combine the lights
		/// </summary>
		public static BlendState BlendBlack = new BlendState()
		{
			ColorBlendFunction = BlendFunction.Add,
			ColorSourceBlend = Blend.One,
			ColorDestinationBlend = Blend.One,

			AlphaBlendFunction = BlendFunction.Add,
			AlphaSourceBlend = Blend.SourceAlpha,
			AlphaDestinationBlend = Blend.One
		};


		
	}
}
