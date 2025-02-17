import React from 'react';

import { usefilesStore } from '../store/usefilesStore';
import FileForm from '../components/fileForm';

const HomePage = () => {
    const { pictures, fetchPictures } = usefilesStore();
    
    return (
        <div>
            <h1>Picture Album</h1>
            <FileForm/>
            <ul>
                {pictures.map(p => <li key={p.id}>{p.name}</li>)}
            </ul>
        </div>
    );
};
