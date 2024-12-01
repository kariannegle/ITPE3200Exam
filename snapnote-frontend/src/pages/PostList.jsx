import React from 'react';
import Post from '../components/Post';

const PostList = ({ posts }) => {
  return (
    <div className="post-feed">
      {posts.length === 0 ? (
        <p>No posts available.</p>
      ) : (
        posts.map((post) => <Post key={post.id} post={post} />)
      )}
    </div>
  );
};

export default PostList;
