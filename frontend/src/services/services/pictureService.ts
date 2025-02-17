import axios from 'axios';

export const addPicture = async (formData: FormData) => {
    return axios.post('/api/pictures', formData);
};
