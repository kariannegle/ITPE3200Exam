import axios from 'axios';

const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';

// Post API calls
export const fetchPosts = async () => {
    const response = await axios.get(`${API_URL}/post`);
    return response.data;
};

export const fetchPostById = async (id) => {
    const response = await axios.get(`${API_URL}/post/${id}`);
    return response.data;
};

export const createPost = async (post) => {
    const response = await axios.post(`${API_URL}/post`, post);
    return response.data;
};

export const updatePost = async (id, post) => {
    const response = await axios.put(`${API_URL}/post/${id}`, post);
    return response.data;
};

export const deletePost = async (id) => {
    const response = await axios.delete(`${API_URL}/post/${id}`);
    return response.data;
};

export const fetchCommentsByPostId = async (postId) => {
    const response = await axios.get(`${API_URL}/comment/byPost/${postId}`);
    return response.data;
};

export const createComment = async (comment) => {
    const response = await axios.post(`${API_URL}/comment`, comment);
    return response.data;
};

export const deleteComment = async (id) => {
    const response = await axios.delete(`${API_URL}/comment/${id}`);
    return response.data;
};

// Account API calls
export const register = async (registerData) => {
    const response = await axios.post(`${API_URL}/account/register`, registerData);
    return response.data;
};

export const login = async (loginData) => {
    const response = await axios.post(`${API_URL}/account/login`, loginData);
    return response.data;
};

export const logout = async () => {
    const response = await axios.post(`${API_URL}/account/logout`);
    return response.data;
};

// User API calls
export const getUserProfile = async (userId) => {
    const response = await axios.get(`${API_URL}/user/profile`, { params: { userId } });
    return response.data;
};

export const updateUserSettings = async (settingsData) => {
    const response = await axios.post(`${API_URL}/user/settings`, settingsData);
    return response.data;
};

// Friend API calls
export const getFriends = async () => {
    const response = await axios.get(`${API_URL}/friend`);
    return response.data;
};

export const searchUsers = async (searchTerm) => {
    const response = await axios.post(`${API_URL}/friend/searchUsers`, { searchTerm });
    return response.data;
};

export const addFriend = async (friendId) => {
    const response = await axios.post(`${API_URL}/friend/addFriend`, { friendId });
    return response.data;
};

export const deleteFriend = async (friendId) => {
    const response = await axios.post(`${API_URL}/friend/deleteFriend`, { friendId });
    return response.data;
};

// Error handling
export const getError = async () => {
    const response = await axios.get(`${API_URL}/error`);
    return response.data;
};