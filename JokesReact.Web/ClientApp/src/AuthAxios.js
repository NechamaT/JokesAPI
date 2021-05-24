  import axios from 'axios';

const getAxios = () => {
    const headers = {};
    const token = localStorage.getItem('auth-token');
    headers['Authorization'] = `Bearer ${token}`;
    headers["foo"] = 'this is weird';
    return axios.create({
        headers
    });
}

export default getAxios;