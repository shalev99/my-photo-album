import React, { useEffect } from 'react';
import { usefilesStore } from '../store/usefilesStore';
import FileForm from '../components/fileForm';

const HomePage = () => {
    const { files, fetchfiles } = usefilesStore();
    
    useEffect(() => {
        fetchfiles();
    }, [fetchfiles]);

    return (
        <div>
            <h1>Picture Album</h1>
            <FileForm />
            <ul>
                {files.map((file) => (
                    <li key={file.id}>
                        <h2>{file.name}</h2>
                        <p><strong>Description:</strong> {file.description}</p>
                        <p><strong>File Name:</strong> {file.fileName}</p>
                        <p><strong>File Size:</strong> {file.fileSize} bytes</p>
                        <p><strong>File Type:</strong> {file.fileType}</p>
                        <p><strong>Upload Date:</strong> {new Date(file.uploadDate).toLocaleString()}</p>
                        <img
                            src={`data:${file.fileType};base64,${file.fileContent}`}
                            alt={file.name}
                            style={{ maxWidth: '300px', height: 'auto' }}
                        />
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default HomePage;
