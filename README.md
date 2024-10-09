  
# Secure File Shredder

### A Windows Forms application for securely deleting sensitive files beyond recovery.

## Overview

Secure File Shredder is a lightweight Windows Forms application built in C#. It provides an easy-to-use interface for securely deleting files from your system using cryptographic random data overwriting techniques, ensuring that your sensitive information cannot be recovered.

The tool utilizes secure overwrite algorithms that perform multiple passes over each file, making it highly resistant to data recovery techniques. This is particularly useful for files that contain private or confidential information, such as passwords, personal documents, or financial data.

## Features

- **Drag-and-Drop Support**: Easily add files for shredding using drag-and-drop.
- **Progress Monitoring**: Visual progress bar and background processing for a responsive user experience.
- **Multiple Overwrite Passes**: Each file is overwritten multiple times with random data to make it difficult to recover.
- **Cancellation Support**: Ability to cancel the shredding process at any time.
- **User-Friendly Interface**: Simple and intuitive UI for ease of use.

## How It Works

The Secure File Shredder application uses the following technique to securely delete files:

1. **File Overwriting**: Each file is overwritten with random data in 3 separate passes. This means the original data is replaced with meaningless data multiple times, making it nearly impossible to recover.
2. **Deletion**: Once the file has been overwritten, it is deleted from the disk.
3. **Cryptographic Random Data**: The application uses the `RNGCryptoServiceProvider` class, a part of the .NET Cryptography namespace, to generate cryptographic random data for file overwriting. This ensures high-quality randomness, making it resistant to forensic analysis.

## Getting Started

### Prerequisites

To run this application, you will need:
 
- **.NET Framework 4.8** or newer.
- **Windows OS**: The application is designed for Windows systems.
 

### Usage

1. **Drag and Drop** the files you want to shred into the application window.
2. Click the **Start Deleting** button to begin the shredding process.
3. Monitor the progress on the **Progress Bar** as files are being securely shredded.
4. If needed, click **Cancel** to terminate the process midway.
5. Once the operation is complete, a confirmation message will be displayed.

### Code Structure

- **`Mainmenu.cs`**: Contains the core logic for the UI, file shredding, and background processing.
- **`ShredFile` Method**: Handles the secure deletion of individual files.
- **`OverwriteFile` Method**: Implements the core logic for cryptographic random data overwriting.
- **`BackgroundWorker`**: Used to process file shredding in the background to keep the UI responsive.

## Working Process

1. **Drag-and-Drop Support**: Users can drag files into the listbox to queue them for shredding.
2. **Background Worker**: The shredding operation is managed using a `BackgroundWorker` to prevent the UI from freezing during long operations.
3. **File Overwriting**:
   - Each file is opened, and random data is written over the file contents.
   - This is done for a specified number of passes (3 by default) to ensure that the original data is completely destroyed.
4. **Progress Reporting**:
   - As each file is processed, the `BackgroundWorker` sends progress updates to the UI, allowing the `ProgressBar` to reflect the current status.
5. **Error Handling**: If any error occurs during the shredding process (e.g., file access denied), the operation is gracefully terminated with a message indicating the issue.

## Future Improvements

- **Advanced Shredding Options**: Allow the user to select the number of passes and the overwrite patterns.
- **Folder Shredding**: Add support for recursively shredding entire directories.
- **Integration with Windows Explorer**: Provide a right-click context menu option for shredding files directly from Windows Explorer.

## Disclaimer

This software is provided for educational purposes only. It is intended to be used for securely deleting files that you own. Misuse of this software to delete unauthorized files is strictly prohibited.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributions

Contributions are welcome! If you have any ideas or bug reports, feel free to open an issue or submit a pull request. 
