using Celeste.Mod.Entities;
using Monocle;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
namespace Celeste.Mod.ClckHelper.Entities;

[CustomEntity("ClckHre/ClckHelper/BrainfuckManager")]
public class BrainfuckManager : Entity
{
    public string flag;
    public string alphabet;
    public int tape_size;
    public int tape_limit;
    BrainfuckInterpreter interpreter;

    public string code;
    public string output = "";
    public BrainfuckManager(EntityData data, Vector2 offset) {
        flag = data.String("flag");
        tape_size = data.Int("tape_size");
        tape_limit = data.Int("tape_limit");
        alphabet = data.String("alphabet");
        Init_Interpreter();
    }

    public BrainfuckInterpreter Init_Interpreter() {
        interpreter = new BrainfuckInterpreter();
        interpreter.tape_size = tape_size;
        interpreter.tape_limit = tape_limit;
        interpreter.alphabet = alphabet;
        return interpreter;

    }
    public void AddCommand(char command) {
        code += command;
    }
    public void RemoveCommand() {
        if (code.Length > 0) {
        code = code.Remove(code.Length - 1, 1);
        }
    }

    public void Interpret() {
        List<int> tape = [];
        for (int i = 0; i < tape_size; i++) {
            tape.Add(0);
        }
        int tape_index = 0;
        int code_index = 0;
        output = "";
        while (code_index < code.Length) {
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
                            if (code[code_index] == '[') {
                                nested += 1;
                            }
                            if (code[code_index] == ']') {
                                nested += -1;
                            }
                        }
                    }
                    ++code_index;
                    break;

                case ']':
                    if (tape[tape_index] != 0) {
                        int nested = -1;
                        while (nested != 0) {
                            --code_index;
                            if (code[code_index] == '[') {
                                nested += 1;
                            }
                            if (code[code_index] == ']') {
                                nested += -1;
                            }
                        }
                    }
                    ++code_index;
                    break;
            }
        }
    }
}

