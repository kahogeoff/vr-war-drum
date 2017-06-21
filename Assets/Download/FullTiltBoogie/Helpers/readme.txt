Instructions:
Preview a particle system by:
A. In Editor View:
a. Selecting it (click it by name, ie 'VfxBoomSparks', in the Hierarchy window.
b. Making it active (checking the top box in the Inspector panel).
c. Pressing Simulate in the particle effect  menu in the Scene window

B. In Game View
i. Press Play to run the scene.
ii. Select a particle system (click it by name in the Hierarchy window).
iii. Making it active by checking the top box in the Inspector panel.

Particles will loop by default. Uncheck the Looping box in the Inspector > Particle System panel and it will only play a single time.

Notes: 
In Unity, the color white is the default, meaning you can't take a coloured PNG file and tint it white. (But you can take a white PNG file and tint it any colour.) So a texture such as the sparks will remain a yellowish orange because that is the color they were saved as in the PNG. You can edit these files in a program such as photoshop to remove or change the color tinting to get the effect you want.
All other color changes can be controlled in the Particle Systems editor panel.

Having issues that should be fixed in future updates? Please let me know at dannymather@gmail.com. Thanks!



Descriptions:

VfxBoomSparks
Hot metal-looking sparks that explode in a hemispherical shape and cool off.

VfxHitSparks
A smaller, more focused version of the spark effect; mimics smaller impacts.

VfxBrightSparks
A brighter, quicker version of the sparks.

VfxBokehColor
Meant to be used to give backgrounds or images life. Set the box dimensions to fill the area you want the bokeh effect to appear in. Adjust colour and timing to suit.

VfxBokehWhite
Same as the colour version but with no colours pre chosen.

VfxDustLoRes
This mimics thousands of dust motes in a circular area by using textures that contain hundreds of dots instead of animating thousands of tiny particles.

VfxCamFlashes
Throw this over a stadium or crowd background to mimic camera flashes effect.

VfxStarsBright
Bright glowing star explosion.

VfxStarsThin
Similar but with less glow applied to the texture, gives more of a glinting effect.

VfxStarsWide
A fat glow applied to the star texture.

VfxSparklesColor
Four pointed stars in a variety of colors, nice for reward animations.

VfxLightningBolts
Color tintable lightning bolts. The sprite sheet contains a few shapes to give this effect a bit of randomness. Great for energy effects, health refills, etc.

VfxBoomFireworks
A simple two-stage fireworks effect. Easy to customize, great for celebratory effects.

Bonus Lens Flare Holder
A fancy little lens flare effect to add to your reward animation or whatnot.


Tips:
All these particles are set to play on awake. This means if they are active they will play.
All these particles are set to loop, meaning they will continue to animate until they are made inactive.