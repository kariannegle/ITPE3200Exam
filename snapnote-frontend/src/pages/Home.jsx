import React, { useState } from 'react';
import CreatePost from '../components/CreatePost';
import PostFeed from '../components/PostFeed';
import { useAuth } from '../context/AuthContext';

const Home = () => {
  const { isAuthenticated } = useAuth(); // Get authentication status

  const [posts, setPosts] = useState([]);

  // Handler for when a new post is created
  const handlePostCreated = (newPost) => {
    setPosts([newPost, ...posts]);
  };

  return (
    <div className="home-page">
      {isAuthenticated ? (
        <CreatePost onPostCreated={handlePostCreated} />
        ) : (
        <>
            <p>Please <a href="/login">log in</a> to create a post.</p>
        </>
        )}

      
      {/* Post feed visible to everyone */}
      <PostFeed posts={posts} />
    </div>
  );
};

export default Home;
