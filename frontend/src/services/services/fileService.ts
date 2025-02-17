import axios from 'axios';

export const addfile = async (formData: FormData) => {
    return axios.post('/api/files', formData);
};