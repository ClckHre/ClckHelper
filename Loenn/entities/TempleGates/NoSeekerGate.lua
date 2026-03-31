local drawableSprite = require("structs.drawable_sprite")
local celesteEnums = require("consts.celeste_enums")
local utils = require("utils")

local NoSeekerGate = {}



local textures = {
    TempleGate_default = "objects/door/TempleDoor00",
    TempleGate_mirror = "objects/door/TempleDoorB00",
    TempleGate_theo = "objects/door/TempleDoorC00"
}

local textureOptions = {}


for texture, _ in pairs(textures) do
    textureOptions[utils.titleCase(texture)] = texture
end

NoSeekerGate.name = "ClckHelper/NoSeekerGate"
NoSeekerGate.depth = -9000
NoSeekerGate.canResize = {false, false}
NoSeekerGate.fieldInformation = {
    sprite = {
        options = textureOptions,
        editable = true
    }
}
NoSeekerGate.placements = {
    name = "NoSeekerGate",
    placementType = "point",
    data = {
        height = 40,
        sprite = "TempleGate_default"
    }
}



function NoSeekerGate.sprite(room, entity)
    local variant = entity.sprite or "default"
    local texture = textures[variant] or textures["default"]
    local sprite = drawableSprite.fromTexture(texture, entity)
    local height = entity.height or 48

    -- Weird offset from the code, justifications are from sprites.xml
    sprite:setJustification(0.5, 0.0)
    sprite:addPosition(4, height - 48)

    return sprite
end

return NoSeekerGate