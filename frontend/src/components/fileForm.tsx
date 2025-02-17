import React from 'react';

const fileForm = () => {
    return (
        <form>
            <input type="text" placeholder="Picture Name" />
            <input type="file" />
            <button type="submit">Add Picture</button>
        </form>
    );
};

export default fileForm;
