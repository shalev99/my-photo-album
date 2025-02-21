import React, { useState, useRef } from 'react';
import { useFilesStore } from '../store/usefilesStore';
import { sanitizeInput, handleFileValidation } from '../utils/utils';

const FileForm = () => {
  const [fileName, setFileName] = useState('');
  const [fileDate, setFileDate] = useState('');
  const [fileDescription, setFileDescription] = useState('');
  const [file, setFile] = useState<File | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [showResetConfirm, setShowResetConfirm] = useState(false);

  const fileInputRef = useRef<HTMLInputElement>(null);
  const addFile = useFilesStore((state) => state.addFile);

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!e.target.files) return;

    const selectedFile = e.target.files[0];
    const validationError = handleFileValidation(selectedFile);

    if (validationError) {
      setErrorMessage(validationError);
      e.target.value = '';
      return;
    }

    setFile(selectedFile);
    setErrorMessage(null);
  };

  const resetForm = () => {
    setFileName('');
    setFileDate('');
    setFileDescription('');
    setFile(null);
    if (fileInputRef.current) {
      fileInputRef.current.value = '';
    }
  };

  const handleConfirmReset = () => {
    resetForm();
    setShowResetConfirm(false);
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!fileName.trim() || !file) {
      setErrorMessage('File name and image file are required!');
      return;
    }

    const sanitizedFileName = sanitizeInput(fileName);
    const sanitizedDescription = sanitizeInput(fileDescription);

    const formData = new FormData();
    formData.append('fileName', sanitizedFileName);
    if (fileDate) formData.append('fileDate', fileDate);
    if (sanitizedDescription) formData.append('fileDescription', sanitizedDescription);
    formData.append('File', file);

    try {
      const response = await fetch('https://localhost:7061/api/files/upload', {
        method: 'POST',
        body: formData,
        headers: { 'X-CSRF-Token': 'your_csrf_token_here' },
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.error || 'Failed to upload file');
      }

      const fileData = await response.json();
      addFile(fileData);

      setSuccessMessage('File uploaded successfully!');
      setTimeout(() => setSuccessMessage(null), 3000);
      resetForm();
    } catch (error: any) {
      console.error('Error uploading file:', error);
      setErrorMessage(error.message || 'An unknown error occurred');
      setTimeout(() => setErrorMessage(null), 3000);
    }
  };

  return (
    <div>
      <form onSubmit={handleSubmit} className="bg-white p-6 rounded-lg shadow-md space-y-4">
        {successMessage && <p className="text-green-600 font-semibold">{successMessage}</p>}
        {errorMessage && <p className="text-red-600 font-semibold">{errorMessage}</p>}

        <div>
          <label htmlFor="fileName" className="block text-sm font-medium">Picture Name:</label>
          <input
            id="fileName"
            type="text"
            value={fileName}
            onChange={(e) => setFileName(e.target.value)}
            maxLength={50}
            required
            className="border rounded-md p-2 w-full"
          />
        </div>

        <div>
          <label htmlFor="fileDate" className="block text-sm font-medium">Picture Date:</label>
          <input
            id="fileDate"
            type="datetime-local"
            value={fileDate}
            onChange={(e) => setFileDate(e.target.value)}
            className="border rounded-md p-2 w-full"
          />
        </div>

        <div>
          <label htmlFor="fileDescription" className="block text-sm font-medium">Picture Description:</label>
          <input
            id="fileDescription"
            type="text"
            value={fileDescription}
            onChange={(e) => setFileDescription(e.target.value)}
            maxLength={250}
            className="border rounded-md p-2 w-full"
          />
        </div>

        <div>
          <label htmlFor="fileInput" className="block text-sm font-medium">Picture File:</label>
          <input
            id="fileInput"
            type="file"
            className="border rounded-md p-2 w-full"
            accept="image/*"
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
            onClick={() => setShowResetConfirm(true)}
            className="bg-gray-500 text-white px-4 py-2 rounded-md hover:bg-gray-600"
          >
            Reset
          </button>
        </div>
      </form>

      {showResetConfirm && (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-30">
          <div className="bg-white p-4 rounded-lg shadow-md">
            <p className="text-lg font-semibold">Are you sure you want to reset the form?</p>
            <div className="flex space-x-2 mt-4">
              <button
                onClick={handleConfirmReset}
                className="bg-red-500 text-white px-4 py-2 rounded-md hover:bg-red-600"
              >
                Yes, Reset
              </button>
              <button
                onClick={() => setShowResetConfirm(false)}
                className="bg-gray-500 text-white px-4 py-2 rounded-md hover:bg-gray-600"
              >
                Cancel
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default FileForm;
