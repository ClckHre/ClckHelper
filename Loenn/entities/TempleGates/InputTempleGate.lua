local drawableSprite = require("structs.drawable_sprite")
local utils = require("utils")

local Gate = {}



local textures = {
    TempleGate_default = "objects/door/TempleDoor00",
    TempleGate_mirror = "objects/door/TempleDoorB00",
    TempleGate_theo = "objects/door/TempleDoorC00"
}

local textureOptions = {}

for texture, _ in pairs(textures) do
    textureOptions[utils.titleCase(texture)] = texture
end

local directionOptions = {
    "DOWN",
    "LEFT",
    "UP",
    "RIGHT"
}

local inputOptions = {
    "Jump",
    "Dash",
    "Grab",
    "Talk",
    "CrouchDash"
}



Gate.name = "ClckHelper/InputTempleGate"
Gate.depth = -9000
Gate.canResize = {false, false}

Gate.fieldOrder = {
    "x", "y", "gate_height", "direction", "open_radius", "close_radius", "input", "sprite", "inverted"
}

Gate.fieldInformation = {
    sprite = {
        options = textureOptions,
        editable = false
    },
    direction = {
        options = directionOptions,
        editable = false
    },
    input = {
        options = inputOptions,
        editable = true
    }
}

Gate.placements = {
    name = "gate",
    placementType = "point",
    data = {
        gate_height = 40,
        sprite = "TempleGate_default",
        open_radius=64,
        close_radius=80,
        inverted=false,
        direction="DOWN",      
        input="Grab",
    }
}




function Gate.sprite(room, entity)
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

return Gate