import React, { useState } from 'react';
import CreatePost from '../components/CreatePost';
import PostFeed from '../components/PostFeed';


const Home = () => {
const [posts, setPosts] = useState([]);

const handlePostCreated = (newPost) => {
    setPosts([newPost, ...posts]);
};

return (
    <div className="home-page">
        <CreatePost onPostCreated={handlePostCreated} />
        <PostFeed posts={posts} />
    </div>
  );
};

export default Home;
