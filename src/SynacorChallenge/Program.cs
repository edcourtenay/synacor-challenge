using SynacorChallenge;

using var reader = new BinaryReader(File.OpenRead("challenge.bin"));

Memory<ushort> memory = new ushort[0x8000];
ushort address = 0;
while (reader.BaseStream.Position < reader.BaseStream.Length)
{
    memory.Span[address++] = reader.ReadUInt16();
}

Memory<ushort> registers = new ushort[0x8];

var vm = new VirtualMachine(memory, registers);

vm.Execute();