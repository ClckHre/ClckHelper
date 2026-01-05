local BrainfuckManager = {}

BrainfuckManager.name = "ClckHre/ClckHelper/BrainfuckManager"
BrainfuckManager.depth = 8998
BrainfuckManager.texture = "characters/theoCrystal/pedestal"
BrainfuckManager.justification = {0.5, 1.0}
BrainfuckManager.placements = {
    name = "BrainfuckManager",
    data = {
        flag="brainfuck_flag",
        tape_size=30000,
        tape_limit=27,
        alphabet=" ABCDEFGHIJKLMNOPQRSTUVWXYZ"
    }
}

return BrainfuckManager