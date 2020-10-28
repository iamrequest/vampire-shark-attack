# Vampire Shark Attack!!!

A stealthy game, where you have to swim through dangerous vampire-shark-infested waters. This was my submission to the ["VR with Andrew's Spooky VR Jam 2020"](https://itch.io/jam/vr-with-andrews-spooky-vr-jam) game jam.

<p align="center">
    <img src="./readmeContents/titlecard.gif">
</p>


You can download the game for free [here, on itch.io](https://request.itch.io/vampire-shark-attack). 

You're free to do whatever with this code, but if you do use it, it'd be real cool of you to link back to this page or the itch.io page (or both). Thanks!

### Some topics of interest in this repo

Ultimately, this was an experiment in a new kind of locomotion. Lately, I've been bored of VR Shooter games, so I've been trying to make games that aren't murder sims.
It's challenging, since making a simple shooter game translates very well to VR!

  * Swimming VR Player locomotion. 
    * TL;DR: The player is moved in the opposite direction from the palm's normal vector. The player is only moved if the palm is facing the same direction as the motion vector
  * Simple finite state machine shark AI (chase player state, patrol state, distracted state).
    * Because the shark is unrestricted in its 3 degrees of motion, it moves via a simple lerp towards target position, slowing as it gets within some radius to avoid overshooting.
  * Simple implementation of Half-Life Alyx's hand containers. Players can store and retrieve Throwable gameobjects in pockets mounted on their wrists.
  
### Next Steps

  * Shark AI
    * The shark motor logic could be improved more than a simple lerp. Since I can't use navmeshes (shark can move vertically, which is a limitation of navmeshes), I'd have to implement an A* pathfinding algorithm.
    * When the shark returns back to the patrol state, it should start patrolling at the nearest Patrol Point in sight, rather than just going back to most recent one.
    * When a shark kills the player, only it is returned to its default state and position. Instead, all sharks should reset to their default state and position.
  * Blood vials
    * These should be tweaked to be more reliable. Regardless of how high I set the radius for Physics.OverlapShere(), sharks would not always be alerted to it. A bit more time could iron out this bug.
  * Settings
    * When I was implementing the swimming locomotion, I had built in functionality to swap in motor presets, in-case players were prone to motion sickness. This would be an arm-mounted UI Canvas menu.
    * Ideally, users could switch between swimming locomotion and traditional smooth locomotion. Users should be able to change how fast and drifty the swimming locomotion is.
    * Ultimately, I couldn't get SteamVR canvas interaction working well enough, so I ended up scrapping it.
  * Juice
    * Even though sharks are silent creatures, they should have some audio SFX to alert the player that they've been spotted/killed.
    * The swimming locomotion should give some audio feedback relative to how fast the player is moving (eg, a bubble sfx)
    * The player could play a heartbeat SFX loop, which would speed up when at least 1 shark has spotted the player

#### Thoughts about the locomotion

The swimming locomotion turned out to be an interesting experiment. It seems like feedback was mixed, in terms of how nausia-inducing it was. 
Because the player's speed was turned down, I found it added to the suspense when a shark was chasing you. I thought the drifty-ness was nice too, since it let you glide through the water a bit.
However, having settings to change how fast the user's motion decays would definitely help prevent motion sickness.

One of the main problems I have with this kind of locomotion, is that it effectively restricts your hands from moving in the scene. Reaching for blood vials, for example, is more frustrating than it should be.
During playtesting, I found myself pushing myself away from whatever I'm trying to grab more often than not. Fortunately, throwing blood vials didn't end up pushing the player around too much, which I was worried about.
Perhaps being able to toggle swimming locomotion on/off via a button press would be a better solution than simply testing Vector3.Dot(palmNormalVector, handMotion)?


### Assets

  * Engine: Unity 2019.4.5f1
	  * URP
  * SFX / BGM
    * [Low Fi, by Pro Sensory](https://opengameart.org/content/low-fi-0), licensed under [CC0](http://creativecommons.org/publicdomain/zero/1.0/)
    * [Door Open/Close, by qubodup](https://opengameart.org/content/door-open-door-close-set), licensed under [CC0](http://creativecommons.org/publicdomain/zero/1.0/)
    * [Horror SFX Library, by Little Robot Sound Factory](https://opengameart.org/content/horror-sound-effects-library), licensed under [CC3.0](http://creativecommons.org/licenses/by/3.0/)
    * [Glass Shatter, by spookymodem](https://opengameart.org/content/breaking-bottle), licensed under [CC3.0](http://creativecommons.org/licenses/by/3.0/)
  * Models
    * [Padlock, by Savino](https://opengameart.org/content/padlock), licensed under [CC0](http://creativecommons.org/publicdomain/zero/1.0/)
    * [Low Poly Shark mesh, by pennomi](https://opengameart.org/content/low-poly-shark), licensed under [CC0](http://creativecommons.org/publicdomain/zero/1.0/)
    * [Graveyard Kit, by Kenney](https://www.kenney.nl/assets/graveyard-kit), licensed under [CC0](http://creativecommons.org/publicdomain/zero/1.0/)
    * [Watercraft Kit, by Kenney](https://www.kenney.nl/assets/watercraft-pack), licensed under [CC0](http://creativecommons.org/publicdomain/zero/1.0/)
    * [Potions and Materials, by Verex](https://www.patreon.com/Verex), licensed under [CC0](http://creativecommons.org/publicdomain/zero/1.0/)
   
