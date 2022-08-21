package ControlGuiLed;

import java.awt.EventQueue;
import java.awt.Rectangle;
import java.awt.RenderingHints.Key;
import java.awt.Robot;
import java.awt.Toolkit;

import javax.swing.JFrame;
import com.fazecast.jSerialComm.SerialPort;

import java.awt.AWTException;
import java.awt.Color;
import java.awt.Desktop;

import javax.swing.JPanel;
import javax.swing.JButton;
import javax.swing.JColorChooser;
import java.awt.event.ActionListener;
import java.awt.event.InputEvent;
import java.awt.event.ActionEvent;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.image.BufferedImage;
import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.Arrays;
import java.util.Timer;
import java.util.TimerTask;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.LinkedBlockingDeque;

import javax.swing.JComboBox;
import javax.swing.JLabel;
import java.awt.event.ItemListener;
import java.awt.event.KeyEvent;
import java.awt.event.ItemEvent;
import javax.swing.JSlider;
import javax.swing.SwingConstants;
import javax.swing.event.ChangeListener;
import javax.swing.event.ChangeEvent;

public class MainForm {

	private JFrame frame;
	private JPanel colorPanel;
	private JComboBox<SerialPort> comboBox;
	private SerialPort _serialPort;
	private Thread readThread;
	private Thread writeThread;
	private Rectangle[] ambilightRectangles;
	BlockingQueue<byte[]> SerialWriteQueue = new LinkedBlockingDeque<>();
	@SuppressWarnings("unused")
	private boolean controlNumFunc;
	private JLabel serialConnectedLabel;
	private int ledBrightness = 255;
	private Timer AmbilightTimer;
	private Timer ColorTimer;
	private Robot robot;
	protected Color selectedColor;
	private int lastColorLedBrightness = -1;
	private Color lastSelectedColor = Color.BLACK;
	Rectangle screenRect = new Rectangle(Toolkit.getDefaultToolkit().getScreenSize());

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					MainForm window = new MainForm();
					window.frame.setVisible(true);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

	/**
	 * Create the application.
	 * 
	 * @throws AWTException
	 */
	public MainForm() throws AWTException {
		initialize();
	}

	public void serialStart() {
		readThread = new Thread(() -> {
			read();
		});
		readThread.start();
		writeThread = new Thread(() -> {
			write();
		});
		writeThread.start();
	}

	private void write() {

		while (true) {
			byte[] data = new byte[0];
			try {
				data = SerialWriteQueue.take();
			} catch (InterruptedException e) {
				e.printStackTrace();
			}
			if (_serialPort != null && _serialPort.isOpen())
				_serialPort.writeBytes(data, data.length);

		}
	}

	public void read() {
		while (true) {
			try {
				if (_serialPort.toString() != comboBox.getItemAt(comboBox.getSelectedIndex()).toString()) {
					resetSerialPort();
					ConnectSerialPort();
				}
				if (_serialPort.bytesAvailable() > 0) {
					byte[] buffer = new byte[1];
					int r = _serialPort.readBytes(buffer, 1);
					int message = buffer[0];
					System.out.println(message);
					switch (message) {
					case Finalants.CONTROL_POWER_SEND_CODE:
						controlNumFunc = !controlNumFunc;
						break;
					case Finalants.CONTROL_VOL_UP_SEND_CODE:

						break;
					case Finalants.CONTROL_FUNC_STOP_SEND_CODE:
						// KeyBD.keybd_event((byte) Keys.Escape, 0, 0, 0);
						break;
					case Finalants.CONTROL_BACKWARDS_SEND_CODE:
						// KeyBD.keybd_event((byte) Keys.MediaPreviousTrack, 0, 0, 0);
						break;
					case Finalants.CONTROL_PLAY_PAUSE_SEND_CODE:
						// KeyBD.keybd_event((byte) Keys.MediaPlayPause, 0, 0, 0);
						break;
					case Finalants.CONTROL_FORWARD_SEND_CODE:
						// KeyBD.keybd_event((byte) Keys.MediaNextTrack, 0, 0, 0);
						break;
					case Finalants.CONTROL_DOWN_SEND_CODE:
						// KeyBD.keybd_event((byte) Keys.Down, 0, 0, 0);
						break;
					case Finalants.CONTROL_VOL_DOWN_SEND_CODE:
						// KeyBD.keybd_event((byte) Keys.VolumeDown, 0, 0, 0);
						break;
					case Finalants.CONTROL_UP_SEND_CODE:
						// KeyBD.keybd_event((byte) Keys.Up, 0, 0, 0);
						break;
					case Finalants.CONTROL_0_SEND_CODE:
						// Haha rickroll
						try {
							Desktop.getDesktop().browse( new URI("https://www.youtube.com/watch?v=dQw4w9WgXcQ"));
						} catch (IOException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						} catch (URISyntaxException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}
						if (!controlNumFunc) {
							// KeyBD.keybd_event((byte) Keys.D0, 0, 0, 0);}\
						} else {

						}
						break;
					case Finalants.CONTROL_EQ_SEND_CODE:
						// KeyBD.keybd_event((byte) Keys.Tab, 0, 0, 0);
						break;
					case Finalants.CONTROL_ST_REPT_SEND_CODE:
						// KeyBD.keybd_event((byte) Keys.Enter, 0, 0, 0);
						break;
					case Finalants.CONTROL_1_SEND_CODE:

						if (!controlNumFunc) {
							// KeyBD.keybd_event((byte) Keys.D1, 0, 0, 0);
						} else {

						}
						break;
					case Finalants.CONTROL_2_SEND_CODE:
						if (!controlNumFunc) {
							// KeyBD.keybd_event((byte) Keys.D2, 0, 0, 0);
						} else {

						}
						break;
					case Finalants.CONTROL_3_SEND_CODE:
						if (!controlNumFunc) {
							// KeyBD.keybd_event((byte) Keys.D3, 0, 0, 0);
						} else {

						}
						break;
					case Finalants.CONTROL_4_SEND_CODE:
						if (!controlNumFunc) {
							// KeyBD.keybd_event((byte) Keys.D4, 0, 0, 0);
						} else {
							// KeyBD.keybd_event((byte) Keys.Left, 0, 0, 0);
						}
						break;
					case Finalants.CONTROL_5_SEND_CODE:
						if (!controlNumFunc) {
							// KeyBD.keybd_event((byte) Keys.D5, 0, 0, 0);
						} else {

						}
						break;
					case Finalants.CONTROL_6_SEND_CODE:
						if (!controlNumFunc) {
							// KeyBD.keybd_event((byte) Keys.D6, 0, 0, 0);
						} else {
							// KeyBD.keybd_event((byte) Keys.Right, 0, 0, 0);
						}
						break;
					case Finalants.CONTROL_7_SEND_CODE:
						if (!controlNumFunc) {
							// KeyBD.keybd_event((byte) Keys.D7, 0, 0, 0);
						} else {

						}
						break;
					case Finalants.CONTROL_8_SEND_CODE:
						if (!controlNumFunc) {
							// KeyBD.keybd_event((byte) Keys.D8, 0, 0, 0);
						} else {

						}
						break;
					case Finalants.CONTROL_9_SEND_CODE:
						if (!controlNumFunc) {
							// KeyBD.keybd_event((byte) Keys.D9, 0, 0, 0);
						} else {

						}
						break;

					}

				}
			}

			catch (NullPointerException ex) {
				ConnectSerialPort();
			}
			try {
				Thread.sleep(40);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}

	private void ConnectSerialPort() {
		while (true) {

			_serialPort = comboBox.getItemAt(comboBox.getSelectedIndex());
			if (_serialPort != null) {
				_serialPort.setComPortParameters(Finalants.BUADRATE, 8, SerialPort.ONE_STOP_BIT, SerialPort.NO_PARITY);
				if (_serialPort.openPort()) {
					serialConnectedLabel.setText("Connected");
					break;
				}

			}
			serialConnectedLabel.setText("Not Connected");

			try {
				Thread.sleep(1000);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}

	public void WriteLedDynamicMode(Color[] color) {
		byte[] data = new byte[Finalants.LEDNUM * 3 + 2];
		data[0] = Finalants.DYNAMIC_RECV_CODE;
		data[1] = (byte) ledBrightness;
		int j = 0;
		for (int i = 2; i < data.length; i += 3) {
			data[i] = (byte) color[j].getRed();
			data[i + 1] = (byte) color[j].getGreen();
			data[i + 2] = (byte) color[j].getBlue();
			j++;
		}
		SerialWriteQueue.add(data);
	}

	public void WriteLedColorMode(Color color, byte brightness) {
		byte[] data = new byte[] { Finalants.COLOR_RECV_CODE, brightness, (byte) color.getRed(),
				(byte) color.getGreen(), (byte) color.getBlue() };
		SerialWriteQueue.add(data);
	}

	/**
	 * Initialize the contents of the frame.
	 * 
	 * @throws AWTException
	 */
	private void initialize() throws AWTException {
		frame = new JFrame();
		frame.setBounds(100, 100, 640, 300);
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		JPanel panel = new JPanel();
		frame.getContentPane().add(panel);

		JButton btnNewButton_1 = new JButton("Choose Color");
		btnNewButton_1.addMouseListener(new MouseAdapter() {
			@Override
			public void mouseClicked(MouseEvent e) {
				Color newColor = JColorChooser.showDialog(null, "Choose a color", Color.RED);
				if (newColor != null) {
					colorPanel.setBackground(newColor);
					colorPanel.setForeground(newColor);
					selectedColor = newColor;
					stopAllTimers();
					ColorTimer = new Timer();
					ColorTimer.scheduleAtFixedRate(new TimerTask() {
						@Override
						public void run() {
							colorTimerTask();
						}
					}, 0, 100);
				}
			}
		});

		panel.add(btnNewButton_1);

		colorPanel = new JPanel();
		colorPanel.setBackground(Color.BLACK);
		colorPanel.setForeground(Color.BLACK);
		panel.add(colorPanel);

		JButton btnNewButton = new JButton("Turn Off");
		btnNewButton.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				colorPanel.setBackground(Color.BLACK);
				colorPanel.setForeground(Color.BLACK);
				selectedColor = Color.BLACK;
				stopAllTimers();
				ColorTimer = new Timer();
				ColorTimer.scheduleAtFixedRate(new TimerTask() {
					@Override
					public void run() {
						colorTimerTask();
					}
				}, 0, 100);
			}

		});
		panel.add(btnNewButton);

		JLabel lblNewLabel = new JLabel("Serial Port:");
		panel.add(lblNewLabel);

		comboBox = new JComboBox<SerialPort>();
		comboBox.addItemListener(new ItemListener() {
			public void itemStateChanged(ItemEvent e) {
				// resetSerialPort();
			}

		});
		panel.add(comboBox);

		JButton refreshSerialButton = new JButton("Refresh Serial Ports");
		refreshSerialButton.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				refreshSerialPorts();
			}
		});
		panel.add(refreshSerialButton);

		serialConnectedLabel = new JLabel("Not Connected");
		panel.add(serialConnectedLabel);

		JButton btnAmbilight = new JButton("Ambilight");
		btnAmbilight.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				stopAllTimers();
				AmbilightTimer = new Timer();
				AmbilightTimer.scheduleAtFixedRate(new TimerTask() {
					@Override
					public void run() {
						ambilightTimerTask();
					}
				}, 0, 40);
			}
		});
		panel.add(btnAmbilight);

		JLabel lblBrightness = new JLabel("Brightness:");
		panel.add(lblBrightness);

		JSlider slider = new JSlider();
		slider.addChangeListener(new ChangeListener() {
			public void stateChanged(ChangeEvent e) {
				ledBrightness = slider.getValue();
				System.out.println(ledBrightness);
			}
		});

		slider.setMaximum(255);
		slider.setValue(255);
		panel.add(slider);
		robot = new Robot();
		ambilightRectangles = new ScreenAmbilightRegions().getRectangles();
		refreshSerialPorts();
		serialStart();
	}

	private void colorTimerTask() {
		if (lastSelectedColor != selectedColor || lastColorLedBrightness != ledBrightness) {
			WriteLedColorMode(selectedColor, (byte) ledBrightness);
			lastColorLedBrightness = ledBrightness;
			lastSelectedColor = selectedColor;
		}
	}

	private void ambilightTimerTask() {
		Color[] pixels = new Color[Finalants.LEDNUM];
		BufferedImage capture = robot.createScreenCapture(screenRect);
		for (int l = 0; l < ambilightRectangles.length; l++) {

			long rSum = 0;
			long gSum = 0;
			long bSum = 0;
			int num = 0;
			for (int i = 0; i < ambilightRectangles[l].width; i += 8) {
				for (int j = 0; j < ambilightRectangles[l].height; j += 8) {

					Color pixel = new Color(capture.getRGB(i + ambilightRectangles[l].x, j + ambilightRectangles[l].y));
					rSum += pixel.getRed();
					gSum += pixel.getGreen();
					bSum += pixel.getBlue();
					num++;
				}
			}

			pixels[l] = new Color((int) (rSum / num), (int) (gSum / num), (int) (bSum / num));

		}
		WriteLedDynamicMode(pixels);
		capture = null;
	}

	private void stopAllTimers() {
		lastColorLedBrightness = -1;
		lastSelectedColor = Color.black;
		if (ColorTimer != null)
			ColorTimer.cancel();
		if (AmbilightTimer != null)
			AmbilightTimer.cancel();
	}

	private void resetSerialPort() {
		if (_serialPort != null) {
			_serialPort.closePort();
			_serialPort = null;
		}

	}

	private void refreshSerialPorts() {
		comboBox.removeAllItems();
		SerialPort[] sps = SerialPort.getCommPorts();
		for (SerialPort sp : sps) {
			comboBox.addItem(sp);
		}
		// resetSerialPort();
	}

	public JPanel getColorPanel() {
		return colorPanel;
	}

	public JComboBox<SerialPort> getComboBox() {
		return comboBox;
	}

	public JLabel getSerialConnectedLabel() {
		return serialConnectedLabel;
	}
}
