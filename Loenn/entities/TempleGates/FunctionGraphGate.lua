local drawableSprite = require("structs.drawable_sprite")
local celesteEnums = require("consts.celeste_enums")
local utils = require("utils")

local FunctionGraphGate = {}



local textures = {
    TempleGate_default = "objects/door/TempleDoor00",
    TempleGate_mirror = "objects/door/TempleDoorB00",
    TempleGate_theo = "objects/door/TempleDoorC00"
}

local textureOptions = {}


for texture, _ in pairs(textures) do
    textureOptions[utils.titleCase(texture)] = texture
end

FunctionGraphGate.name = "ClckHelper/FunctionGraphGate"
FunctionGraphGate.depth = -9000
FunctionGraphGate.canResize = {false, false}
FunctionGraphGate.fieldInformation = {
    sprite = {
        options = textureOptions,
        editable = true
    }
}
FunctionGraphGate.placements = {
    name = "FunctionGraphGate",
    placementType = "point",
    data = {
        height = 40,
        sprite = "TempleGate_default"
    }
}



function FunctionGraphGate.sprite(room, entity)
    local variant = entity.sprite or "default"
    local texture = textures[variant] or textures["default"]
    local sprite = drawableSprite.fromTexture(texture, entity)
    local height = entity.height or 48

    -- Weird offset from the code, justifications are from sprites.xml
    sprite:setJustification(0.5, 0.0)
    sprite:addPosition(4, height - 48)

    return sprite
end

return FunctionGraphGate