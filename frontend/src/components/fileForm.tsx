import React, { useState, useRef } from 'react';
import { usefilesStore } from '../store/usefilesStore';

const FileForm = () => {
  // State variables for form inputs
  const [fileName, setfileName] = useState('');
  const [fileDate, setfileDate] = useState('');
  const [fileDescription, setfileDescription] = useState('');
  const [file, setFile] = useState<File | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  // Reference to the file input for resetting its value
  const fileInputRef = useRef<HTMLInputElement>(null);

  // Get the addFile function from the Zustand store
  const addFile = usefilesStore((state) => state.addFile);

  // Handle file selection from the input
  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      setFile(e.target.files[0]);
    }
  };

  // Reset the form fields
  const resetForm = () => {
    setfileName('');
    setfileDate('');
    setfileDescription('');
    setFile(null);
    if (fileInputRef.current) {
      fileInputRef.current.value = '';
    }
  };

  // Handle form submission and file upload
  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!fileName || !file) {
      alert('file Name and  File are mandatory!');
      return;
    }

    const formData = new FormData();
    formData.append('fileName', fileName);
    if (fileDate) formData.append('fileDate', fileDate); // Only append if not empty
    if (fileDescription) formData.append('fileDescription', fileDescription); // Only append if not empty
    formData.append('File', file);

    try {
      const response = await fetch('https://localhost:7061/api/files/upload', {
        method: 'POST',
        body: formData,
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.error || 'Failed to upload file');
      }

      const fileData = await response.json();
      console.log('Response data:', fileData);  // Log the response to check for 'Src'

      addFile(fileData);

      setSuccessMessage('file uploaded successfully!');
      setTimeout(() => setSuccessMessage(null), 3000);

      resetForm();
    } catch (error: any) {
      console.error('Error uploading file:', error);
      setErrorMessage(error.message || 'An unknown error occurred');
      setTimeout(() => setErrorMessage(null), 3000);

    }
  };

  return (
    <form onSubmit={handleSubmit} className="bg-white p-6 rounded-lg shadow-md space-y-4">
      {successMessage && <p className="text-green-600 font-semibold">{successMessage}</p>}
      {errorMessage && <p className="text-red-600 font-semibold">{errorMessage}</p>}

      <div>
        <label className="block text-sm font-medium">Picture Name:</label>
        <input
          type="text"
          value={fileName}
          onChange={(e) => setfileName(e.target.value)}
          maxLength={50}
          required
          className="border rounded-md p-2 w-full"
        />
      </div>

      <div>
        <label className="block text-sm font-medium">Picture Date:</label>
        <input
          type="datetime-local"
          value={fileDate}
          onChange={(e) => setfileDate(e.target.value)}
          className="border rounded-md p-2 w-full"
        />
      </div>

      <div>
        <label className="block text-sm font-medium">Picture Description:</label>
        <input
          type="text"
          value={fileDescription}
          onChange={(e) => setfileDescription(e.target.value)}
          maxLength={250}
          className="border rounded-md p-2 w-full"
        />
      </div>

      <div>
        <label className="block text-sm font-medium">Picture File:</label>
        <input
          type="file"
          className="border rounded-md p-2 w-full"
          onChange={handleFileChange}
          required
          ref={fileInputRef}
        />
      </div>

      <div className="flex space-x-2">
        <button
          type="submit"
          className="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600"
        >
          Add Picture
        </button>
        <button
          type="button"
          onClick={resetForm}
          className="bg-gray-500 text-white px-4 py-2 rounded-md hover:bg-gray-600"
        >
          Reset
        </button>
      </div>
    </form>
  );
};

export default FileForm;
