using System.Collections.Generic;
using System.Threading.Tasks;

namespace Celeste.Mod.ClckHelper;


public class BrainfuckInterpreter {
    List<int> tape = [];
    public string code = "";
    public string output = "";

    public int code_index = 0;
    public int tape_index = 0;
    public int time = 0;
    bool interpreting = false;
    public int tape_size;
    public int tape_limit;
    public string alphabet;
    public BrainfuckInterpreter() {
        for (int i = 0; i < tape_size; i++) {
        tape.Add(0);
        }
    }
    public void reset_tape() {
        for (int i = 0; i < tape_size; i++) {
        tape[i] = 0;
        }
    }
    public async Task interpret(string new_code) {
        interpreting = false;
        code = new_code;
        output = "";
        reset_tape();
        code_index = 0;
        tape_index = 0;
        time = 0;
        interpreting = true;

        while (code_index < code.Length && interpreting) {
            step();
            await Task.Delay(250);
        }
    }
    public void step() {
        switch(code[code_index]) {
            case '+':
                tape[tape_index] = ++tape[tape_index];
                if (tape[tape_index] >= tape_limit) tape[tape_index] = 0;
                ++code_index;
                break;

            case '-':
                tape[tape_index] = --tape[tape_index];
                if (tape[tape_index] < 0) tape[tape_index] = tape_limit - 1;
                ++code_index;
                break;

            case '<':
                tape_index = --tape_index;
                if (tape_index < 0) tape_index = tape_size - 1;
                ++code_index;
                break;

            case '>':
                tape_index = ++tape_index;
                if (tape_index >= tape_size) tape_index = 0;
                ++code_index;
                break;

            case '.':
                output += alphabet[tape[tape_index]];
                ++code_index;
                break;

            case '[':
                if (tape[tape_index] == 0) {
                    int nested = 1;
                    while (nested != 0) {
                        ++code_index;
                        if (code[code_index] == '[') nested += 1;
                        if (code[code_index] == ']') nested += -1;
                    }
                }
                ++code_index;
                break;

            case ']':
                if (tape[tape_index] != 0) {
                    int nested = -1;
                    while (nested != 0) {
                        --code_index;
                        if (code[code_index] == '[') nested += 1;
                        if (code[code_index] == ']') nested += -1;
                    }
                }
                ++code_index;
                break;
        }
        ++time;
    } 
}