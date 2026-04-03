local drawableSprite = require("structs.drawable_sprite")
local celesteEnums = require("consts.celeste_enums")
local utils = require("utils")

local EntityNearbyGate = {}



local textures = {
    TempleGate_default = "objects/door/TempleDoor00",
    TempleGate_mirror = "objects/door/TempleDoorB00",
    TempleGate_theo = "objects/door/TempleDoorC00"
}

local textureOptions = {}


for texture, _ in pairs(textures) do
    textureOptions[utils.titleCase(texture)] = texture
end

EntityNearbyGate.name = "ClckHelper/EntityNearbyGate"
EntityNearbyGate.depth = -9000
EntityNearbyGate.canResize = {false, false}
EntityNearbyGate.fieldInformation = {
    sprite = {
        options = textureOptions,
        editable = true
    }
}
EntityNearbyGate.placements = {
    name = "EntityNearbyGate",
    placementType = "point",
    data = {
        gate_height = 40 = 40,
        sprite = "TempleGate_theo",
        SID="theoCrystal",
        open_radius=64,
        close_radius=80,
        inverted=false
    }
}



function EntityNearbyGate.sprite(room, entity)
    local variant = entity.sprite or "default"
    local texture = textures[variant] or textures["default"]
    local sprite = drawableSprite.fromTexture(texture, entity)
    local height = entity.height or 48

    -- Weird offset from the code, justifications are from sprites.xml
    sprite:setJustification(0.5, 0.0)
    sprite:addPosition(4, height - 48)

    return sprite
end

return EntityNearbyGate