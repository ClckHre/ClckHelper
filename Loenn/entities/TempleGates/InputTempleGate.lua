local drawableSprite = require("structs.drawable_sprite")
local celesteEnums = require("consts.celeste_enums")
local utils = require("utils")

local InputTempleGate = {}



local textures = {
    TempleGate_default = "objects/door/TempleDoor00",
    TempleGate_mirror = "objects/door/TempleDoorB00",
    TempleGate_theo = "objects/door/TempleDoorC00"
}

local textureOptions = {}

local directionOptions = {
    "DOWN",
    "LEFT",
    "UP",
    "RIGHT"
}

for texture, _ in pairs(textures) do
    textureOptions[utils.titleCase(texture)] = texture
end

InputTempleGate.name = "ClckHelper/InputTempleGate"
InputTempleGate.depth = -9000
InputTempleGate.canResize = {false, false}
InputTempleGate.fieldInformation = {
    sprite = {
        options = textureOptions,
        editable = true
    },
    direction = {
        options = directionOptions,
        editable = false
    }
}
InputTempleGate.placements = {
    name = "InputTempleGate",
    placementType = "point",
    data = {
        gate_height = 40,
        sprite = "TempleGate_default",
        inverted=false,
        controller_button="Y",
        key="R",
        direction="DOWN"
    }
}




function InputTempleGate.sprite(room, entity)
    local direction = entity.direction or "DOWN"
    local variant = entity.sprite or "default"
    local texture = textures[variant] or textures["default"]
    local sprite = drawableSprite.fromTexture(texture, entity)
    local height = entity.gate_height or 48
    if direction == "UP" then
        sprite.rotation = math.pi
        sprite:addPosition(4, 48 - height)
    elseif direction == "RIGHT" then
        sprite.rotation = -math.pi/2
        sprite:addPosition(height - 48, 4)
    elseif direction == "LEFT" then
        sprite.rotation = math.pi/2
        sprite:addPosition(48 - height, 4)
    else
        sprite.rotation = 0
        sprite:addPosition(4, height - 48)
    end
    -- Weird offset from the code, justifications are from sprites.xml
    sprite:setJustification(0.5, 0.0)
    

    return sprite
end

return InputTempleGate