# GamedevelopHer: Principles of Programming - MVC Unity Project

> [!Note]  
> This project is part of the **GamedevelopHer: Principles of Programming** course.  
> It contains intentional design flaws and bugs that are explored, addressed, and fixed throughout course lessons and homework assignments.

## Overview

This project serves as an example of implementing a Model-View-Controller (MVC) architecture in Unity, with a focus on teaching fundamental programming principles.

## Design Choices

This project follows two key design principles:

1. **Unity-Only Tools**  
   - All components are built using Unity’s built-in tools, avoiding any additional plugins, NuGet packages, or external DLLs.

2. **View-Only MonoBehaviours**  
   - Only the View classes inherit from `MonoBehaviour`, keeping the business logic separate and maintaining clean design patterns.  
   - Occasionally, some logical components may inherit from `MonoBehaviour` to support certain design patterns effectively.

