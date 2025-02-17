import React, { useState } from 'react';

const FileForm = () => {
    // Explicitly define the type of pictureFile as File | null
    const [pictureName, setPictureName] = useState('');
    const [pictureDate, setPictureDate] = useState('');
    const [pictureDescription, setPictureDescription] = useState('');
    const [pictureFile, setPictureFile] = useState<File | null>(null); // Updated type

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files) {
            setPictureFile(e.target.files[0]);
        }
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!pictureName || !pictureFile) {
            alert('Picture Name and Picture File are mandatory!');
            return;
        }

        // Prepare form data for submission
        const formData = new FormData();
        formData.append('pictureName', pictureName);
        formData.append('pictureDate', pictureDate);
        formData.append('pictureDescription', pictureDescription);
        formData.append('pictureFile', pictureFile); // pictureFile is a File type

        // Send data to the server
        fetch('https://localhost:7061/api/files', {
            method: 'POST',
            body: formData,
        })
        .then((response) => response.json())
        .then((data) => {
            console.log('Picture uploaded successfully:', data);
        })
        .catch((error) => {
            console.error('Error uploading picture:', error);
        });
    };

    const handleReset = () => {
        setPictureName('');
        setPictureDate('');
        setPictureDescription('');
        setPictureFile(null); // Reset to null
    };

    return (
        <form onSubmit={handleSubmit}>
            <div>
                <label>Picture Name:</label>
                <input
                    type="text"
                    value={pictureName}
                    onChange={(e) => setPictureName(e.target.value)}
                    maxLength={50}
                    required
                    placeholder="Enter picture name"
                />
            </div>

            <div>
                <label>Picture Date:</label>
                <input
                    type="datetime-local"
                    value={pictureDate}
                    onChange={(e) => setPictureDate(e.target.value)}
                />
            </div>

            <div>
                <label>Picture Description:</label>
                <input
                    type="text"
                    value={pictureDescription}
                    onChange={(e) => setPictureDescription(e.target.value)}
                    maxLength={250}
                    placeholder="Enter picture description"
                />
            </div>

            <div>
                <label>Picture File:</label>
                <input
                    type="text"
                    value={pictureFile ? pictureFile.name : ''}
                    readOnly
                    placeholder="No file selected"
                />
            </div>

            <div>
                <button
                    type="button"
                    onClick={() => document.getElementById('fileInput')?.click()}
                >
                    Picture Browser
                </button>
                <input
                    id="fileInput"
                    type="file"
                    style={{ display: 'none' }}
                    onChange={handleFileChange}
                    required
                />
            </div>

            <div>
                <button type="submit">Add Picture</button>
                <button type="button" onClick={handleReset}>
                    Reset
                </button>
            </div>
        </form>
    );
};

export default FileForm;
