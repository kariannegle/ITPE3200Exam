// src/components/UserPosts.jsx
import React, { useEffect, useState } from 'react';
import Post from './Post.jsx'; // Import your existing Post component

const UserPosts = ({ userId }) => {
  const [userPosts, setUserPosts] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUserPosts = async () => {
      try {
        const response = await fetch(`http://localhost:5000/api/user/${userId}/posts`);
        if (response.ok) {
          const data = await response.json();
          setUserPosts(data);
        } else {
          console.error('Failed to fetch user posts:', response.status);
        }
      } catch (error) {
        console.error('An error occurred while fetching user posts:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchUserPosts();
  }, [userId]);

  if (loading) {
    return <p>Loading posts...</p>;
  }

  return (
    <div className="user-posts">
      {userPosts.length > 0 ? (
        userPosts.map((post) => (
          <Post key={post.id} post={post} />
        ))
      ) : (
        <p>No posts available for this user.</p>
      )}
    </div>
  );
};

export default UserPosts;
