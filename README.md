 
# Secure File Shredder

### A Windows Forms application for securely deleting sensitive files beyond recovery.

## Overview

**Secure File Shredder** is a lightweight Windows Forms application built in C#. It provides an intuitive interface for securely deleting files from your system using cryptographic random data overwriting techniques, ensuring that sensitive information cannot be recovered.

![image](https://github.com/user-attachments/assets/eba6ce89-4a9a-48ad-9cc7-39116c0e6490)


The tool utilizes secure overwrite algorithms with multiple passes, making it highly resistant to data recovery techniques. It’s perfect for securely deleting files that contain personal, confidential, or financial information.

## Features

- **Drag-and-Drop Support**: Easily drag files into the application for shredding.
- **Progress Monitoring**: Displays progress via a progress bar with background processing for responsiveness.
- **Multiple Overwrite Passes**: Files are overwritten multiple times with random data for secure deletion.
- **Cancellation Support**: Shredding can be canceled at any time during the process.
- **User-Friendly Interface**: Designed for simplicity and ease of use.
- **Error Handling and Logging**: Clear error messages and logging support for tracking issues.

## How It Works

The Secure File Shredder application securely deletes files using the following technique:

1. **File Overwriting**: Files are overwritten with random data in 3 passes to make recovery nearly impossible.
2. **Deletion**: Files are removed from the disk once they have been securely overwritten.
3. **Cryptographic Random Data**: The `RNGCryptoServiceProvider` class from the .NET Cryptography namespace generates secure random data for file overwriting.

## Getting Started

### Prerequisites

- **.NET Framework 4.8** or newer
- **Windows OS**: Application designed for Windows environments.

### Usage

1. **Drag and Drop** files into the application window.
2. Click **Start Deleting** to shred the selected files.
3. Monitor the progress via the **Progress Bar**.
4. Click **Cancel** if you wish to stop the shredding process.
5. Once the operation is complete, a confirmation message will appear.

## Code Structure

- **`Mainmenu.cs`**: Contains the core UI logic, file shredding functionality, and background processing.
- **`ShredFile` Method**: Handles secure deletion of each file.
- **`OverwriteFile` Method**: Implements cryptographic random data overwriting logic.
- **`BackgroundWorker`**: Keeps the UI responsive during the shredding process.

## Working Process

1. **Drag-and-Drop Support**: Add files to the shredding queue via drag and drop.
2. **Background Worker**: Uses `BackgroundWorker` to manage shredding without freezing the UI.
3. **File Overwriting**: Files are overwritten with random data for multiple passes (default is 3 passes).
4. **Progress Reporting**: Displays progress updates in the UI while files are being processed.
5. **Error Handling**: Gracefully handles errors like file access issues and provides clear feedback to users.

## Future Enhancements

Some possible future improvements include:

1. **Advanced Shredding Options**: Support for additional algorithms like **Gutmann (35 passes)** or **DoD 5220.22-M (7 passes)**.
2. **Folder Shredding**: Enable shredding of entire directories, including subfolders.
3. **Windows Shell Integration**: Add a right-click option in Windows Explorer to shred files directly.
4. **Secure Free Space Wiping**: Overwrite free space to ensure recovery of previously deleted files is impossible.
5. **Shredding Locked Files**: Support shredding of files in use or locked by other processes.
6. **Metadata Shredding**: Securely erase file metadata (e.g., file name, timestamps).
7. **Multi-Threaded Shredding**: Speed up the process by shredding multiple files concurrently.
8. **Password Protection**: Add password protection to prevent unauthorized access.
9. **Encrypted File Shredding**: Add an encryption step before shredding for added security.
10. **Shredding from External Storage**: Support shredding files from USB drives and external HDDs.
11. **Panic Button**: A button that instantly stops the shredding operation.
12. **Shredding Profiles**: Save and reuse specific shredding settings as profiles.
13. **File Type Filters**: Shred only selected file types (e.g., `.pdf`, `.docx`).
14. **Detailed Logging**: Maintain logs of shredded files, including timestamps and algorithms used.
15. **Command-Line Interface (CLI) Support**: Provide a CLI version for advanced users and batch processing.

## Disclaimer

This software is provided for educational purposes only. Use it to securely delete files that you own. Deleting unauthorized files is prohibited.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.txt) file for details.

## Contributions

Contributions are welcome! Feel free to open an issue or submit a pull request with your improvements or bug fixes.
 
