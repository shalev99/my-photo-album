import React from 'react';

const PictureForm = () => {
    return (
        <form>
            <input type="text" placeholder="Picture Name" />
            <input type="file" />
            <button type="submit">Add Picture</button>
        </form>
    );
};

export default PictureForm;
