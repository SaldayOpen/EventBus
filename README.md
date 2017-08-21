# EventBus
Salday EventBus is a system, which could dispatch published events to any registered handler accepting particular event argument. This means, that your event sources should no longer contain references to other components, only to event bus. 

This creates a clean separation, between event sources and handlers, and allows to build handler pipelines dynamically in runtime, rather than compile time. So in theory you can even load third-party libraries adopted for this Event Bus and insert their handlers into the pipeline. For example, this behavior can be used to implement plugin systems.

## Features
- Custom event arguments.
- Automatic event data mapping to handlers by type.
- Ability to decouple code.
- Prioritized handlers.
- Cancelable events.
- Event pipeline building in runtime.

## Unity
There is an asset for Unity game engine based on EventBus, called EventMinion. It is available for free on the [Asset Store](https://www.assetstore.unity3d.com/en/#!/content/78088)
