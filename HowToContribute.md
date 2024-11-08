# How to contribute
The unity project is built to be very modular. Theoretically, creating a new transmutation or implementing a new kind of tile *SHOULD* be a very self-contained process.

## Transmutation summary
The core mechanic of the game is **Transmutation**. Transmutation is a "spellcasting"-ish system that gives the player various spells that can only be cast when near certain types of terrain. The logic can be broken down into this:
- The world is composed of **Tiles** on a grid.
- Each **Tile** is associated with a **TileData**.
- Each **TileData** has two associated **Materials**.
  - A **Supermaterial**: A more *broad* classification of tile material. Ground, Plant, Liquid are examples of this. Determines the transmutation in the first slot.
  - A **Submaterial**: The *specific* type of tile material. Dirt, Grass, Stone, Metal, Water would be examples of this. Determines the transmutations in the second, third, and fourth slots. 
- Each **Material** has associated **Transmutations**.
- Each **Transmutation** has an individual script that details its effects.
The player has four transmutation slots. The transmutation cast by each slot changes depending on the current tile's materials. The player starts out knowing very few transmutations (possibly one) and can unlock more with spell scrolls.

## The project structure
The construction of this project makes use of **ScriptableObjects**, a Unity feature that allows you to write classes that can then be instantiated into assets. These assets hold the data for transmutations, materials, and tile types.
- **TileData** instances contain a list of specific **Tiles** that it applies to. It also contains a **Supermaterial** and a **Submaterial**
- **MaterialData** instances contain boolean values that specify what slots the material has transmutations for, and up to four **Transmutations** that are associated with the material.
- **TransmutationSOBase** instances are a bit more complicated, but contain the code for individual **Transmutations.**

Writing a **Transmutation** script, instantiating it into a **TransmutationSOBase** object, giving that object to a **MaterialData** object, and giving that **MaterialData** object to a **TileData** object allows for that transmutation to be cast when the player is standing on a tile specified in the **TileData** object.
