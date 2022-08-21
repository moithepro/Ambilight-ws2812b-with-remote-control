package ControlGuiLed;

public class Color {
	private byte r;
	private byte g;
	private byte b;

	public byte getB() {
		return b;
	}

	public byte getG() {
		return g;
	}

	public byte getR() {
		return r;
	}

	public Color(byte r, byte g, byte b) {
		super();
		this.r = r;
		this.g = g;
		this.b = b;
	}

	public Color(int hex) {
		b = (byte) ((hex) & 0xFF);
		g = (byte) ((hex >> 8) & 0xFF);
		r = (byte) ((hex >> 16) & 0xFF);
	}
}
