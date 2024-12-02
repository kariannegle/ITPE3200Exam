import axios from 'axios';

const instance = axios.create({
  baseURL: 'http://your-api-base-url.com', // Replace with your API base URL
  headers: {
    'Content-Type': 'application/json',
  },
});

export default instance;