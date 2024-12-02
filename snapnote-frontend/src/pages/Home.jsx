// src/pages/Home.jsx
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

  // Handler for deleting a post
  const handleDeletePost = (postId) => {
    const confirmed = window.confirm("Are you sure you want to delete this post?");
    if (confirmed) {
      setPosts(prevPosts => prevPosts.filter(post => post.id !== postId));
      console.log(`Post with ID ${postId} has been deleted.`);
    }
  };

  return (
    <div className="home-page">
      {isAuthenticated ? (
        <CreatePost onPostCreated={handlePostCreated} />
      ) : (
        <p>
          Please <a href="/login">log in</a> to create a post.
        </p>
      )}

      {/* Post feed visible to everyone */}
      <PostFeed posts={posts} onDelete={handleDeletePost} />
    </div>
  );
};

export default Home;
