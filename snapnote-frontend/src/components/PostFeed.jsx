// src/components/PostFeed.jsx
import React from 'react';
import PropTypes from 'prop-types';
import Post from '../components/Post'; // Ensure the path is correct

const PostFeed = ({ posts, onDelete }) => {
  console.log("PostFeed received onDelete:", typeof onDelete);

  if (typeof onDelete !== 'function') {
    console.error("The onDelete function is missing in PostFeed!");
    return null; // Optionally, render a fallback UI
  }

  const handleEdit = (postId, newContent) => {
    console.log(`Editing post ${postId} with new content: ${newContent}`);
    
  };
  const handleAddComment = (postId, comment) => {
    console.log(`Adding comment to post ${postId}:`, comment);
    // Logic to handle adding a comment
  };
  const handleDeleteComment = (postId, commentId) => {
    console.log(`Deleting comment ${commentId} from post ${postId}`);
    // Logic to handle deleting a comment
  };
  return (
    <div className="post-feed">
      {posts && posts.length > 0 ? (
        posts.map(post => (
          <Post
            key={post.id}
            post={post}
            onDelete={onDelete}
            onEdit={handleEdit} // Pass the handleEdit function to the Post component
            onAddComment={handleAddComment}
            onDeleteComment={handleDeleteComment}
          />
        ))
      ) : (
        <p>No posts available.</p>
      )}
    </div>
  );
};

PostFeed.propTypes = {
  posts: PropTypes.arrayOf(PropTypes.object).isRequired,
  onDelete: PropTypes.func.isRequired,
};

export default PostFeed;