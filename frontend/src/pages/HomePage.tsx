import React from 'react';

import { usePictureStore } from '../store/usePictureStore';
import PictureForm from '../components/PictureForm';

const HomePage = () => {
    const { pictures, fetchPictures } = usePictureStore();
    
    return (
        <div>
            <h1>Picture Album</h1>
            <PictureForm />
            <ul>
                {pictures.map(p => <li key={p.id}>{p.name}</li>)}
            </ul>
        </div>
    );
};
