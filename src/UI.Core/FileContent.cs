namespace UI.Core;

public readonly record struct FileContent(string Name, byte[] Content, Size Size);