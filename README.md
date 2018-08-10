<h1 align="center">
    <img src="https://raw.githubusercontent.com/BrunoSilvaFreire/ShiroiFX/master/ShiroiFX-Assets/Logo.png" alt="ShiroiFX" width="400">
</h1>

<p align="center">
    <img src="https://img.shields.io/github/license/BrunoSilvaFreire/ShiroiFX.svg">
    <img src="https://img.shields.io/github/last-commit/BrunoSilvaFreire/ShiroiFX.svg">
</p>  
<p align="middle">
    <img src="https://unity3d.com/profiles/unity3d/themes/unity/images/company/brand/logos/primary/unity-master-black.svg" width="200">
</p>  

## A library with Game Feel and ease of use, your designers will love you forever   
# ShiroiFX
This library was made to make creating game feel and effects easily.
## Features
* Created to be absurdly easy to use.
* Ever wanted to affect something with multiple different values 
and then blend then all together in the final result? We present to you **Services**!
* Documented 
## Quick Examples
### Effects
```csharp
public class Projectile : MonoBehaviour {
    public WorldEffect OnHit;
    public WorldEffect OnShot;

    public void Shoot(Entity owner) {
        // TODO: Execute launch here
        // Null safe extension method, only executes if effect is not null
        OnShot.ExecuteIfPresent(transform.position);
    }

    // Bla bla bla
        
    private void OnHitSomething(Collision col) {
        // TODO: Apply damage somehow
        var pos = col.contacts.First().point;
        OnHit.ExecuteIfPresent(pos);
    }
}
```
### Services
```csharp
var timeController = new GameObject().AddComponent<TimeController>();
var curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
var serviceA = timeController.RegisterContinualService(meta: new ConstantTimeMeta(2), priority: 10);
var serviceB = timeController.RegisterTimedService(duration: 5, meta: new AnimatedTimeMeta(curve), priority: 20);
/* 
 * Services A and B are automatically blended by the   
 * TimeController based on their priorities
 */
```
## Getting Started  

Please refer to the [Wiki](https://github.com/DDevilISL/ShiroiFX/wiki) for instructions on how to use ShiroiFX

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details