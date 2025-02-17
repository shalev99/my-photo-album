import React from 'react';

import { usefilesStore } from '../store/usefilesStore';
import FileForm from '../components/fileForm';
import { useEffect } from 'react';

const HomePage = () => {
    const { files: files, fetchfiles: fetchfiles } = usefilesStore();
    useEffect(() => {
        fetchfiles();  
      }, []);
    return (
        <div>
            <h1>Album:</h1>
            <FileForm/>
            <ul>
                {files.map(p => <li key={p.id}>{p.name}</li>)}
            </ul>
        </div>
    );
};

export default HomePage;